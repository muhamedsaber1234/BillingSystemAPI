﻿using Billing_System.BuissnessLogic.DTO.Client;
using Billing_System.BuissnessLogic.DTO.Item;
using Billing_System.BuissnessLogic.Interfaces;
using BillingSystem.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using model.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Billing_System.BuissnessLogic.Services
{
    public class Itemervices:IItemService
    {
        public readonly IUnitOfWork context;
        public Itemervices(IUnitOfWork _context)
        {
            context = _context;
        }
        public void Add(ItemToAddDto item)
        {
            if (item == null)
                throw new Exception("item Cant Be Null");
            var iteminDb = context.Items.GetAll().Where(c => c.Name == item.Name).FirstOrDefault();
            if (iteminDb != null)
                throw new Exception("Items already exists in database");
            if (item.SellingPrice <= item.BuyingPrice)
                throw new Exception("selling price must be greater than buying price");
            context.Items.Add(new Item
            {
                Name = item.Name,
                Note = item.Note,
                SellingPrice = item.SellingPrice,
                BuyingPrice = item.BuyingPrice,
                AvailableQyantity = item.AvailableQyantity,
                TypeId = item.TypeId,
                UnitId = item.UnitId,
                CompanyId = item.CompanyId

            });
            context.Complete();
        }
        public List<ItemToReturnDto> GetAll()
        { 
        var items = context.Items.GetAll();
        var itemstoreturn = new List<ItemToReturnDto>();
            foreach (var item in items)
            {
                itemstoreturn.Add(new ItemToReturnDto
                {
                    Id = item.Id,
                    Name = item.Name,
                    Note = item.Note,
                    SellingPrice = item.SellingPrice,
                    BuyingPrice = item.BuyingPrice,
                    AvailableQyantity = item.AvailableQyantity,
                    TypeId = item.TypeId,
                    UnitId = item.TypeId,
                    CompanyId = item.TypeId
                }
                    );
            }
            return itemstoreturn;
        }
        public ItemToReturnDto GetById(int id)
        {
            var item = context.Items.GetById(id);
            if (item == null)
                throw new Exception("this item is not found plz enter a valid Item");
            var itemtoreturn = new ItemToReturnDto
            {
                Id = item.Id,
                Name = item.Name,
                Note = item.Note,
                SellingPrice = item.SellingPrice,
                BuyingPrice = item.BuyingPrice,
                AvailableQyantity = item.AvailableQyantity,
                TypeId = item.TypeId,
                UnitId = item.UnitId,
                CompanyId = item.CompanyId
            };
            return itemtoreturn;
        }
        public void DeleteById(int id)
        {
            var item = context.Items.GetById(id);
            if (item == null)
                throw new Exception("Item is not exist");
            context.Items.Delete(item);
            context.Complete(); 
        }
        public void Edit(int id,ItemToAddDto item)
        {
            if (item == null)
                throw new Exception("item is empaty(null)");
            var itemINDb =context.Items.GetById(id);
            if (itemINDb == null) 
                throw new Exception("this item is not exist");
            var itemDuplicationName = context.Items.GetAll().Where(x => x.Name == item.Name).FirstOrDefault();
            if (itemDuplicationName != null)
                throw new Exception("this item's name is already exist");
            itemINDb.Name = item.Name;
            itemINDb.Note = item.Note;
            itemINDb.UnitId = item.UnitId;
            itemINDb.SellingPrice = item.SellingPrice;
            itemINDb.BuyingPrice = item.BuyingPrice;
            itemINDb.AvailableQyantity = item.AvailableQyantity;
            itemINDb.CompanyId = item.CompanyId;
            itemINDb.TypeId = item.TypeId;

            context.Items.Update(itemINDb);
            context.Complete();
        }
    }
}

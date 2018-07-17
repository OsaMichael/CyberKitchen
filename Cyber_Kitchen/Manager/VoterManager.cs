﻿using Cyber_Kitchen.Interface;
using Cyber_Kitchen.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.IO;
using Cyber_Kitchen.Interface.Utils;
using Cyber_Kitchen.Entities;

namespace Cyber_Kitchen.Manager
{
    public class VoterManager : IVoterManager
    {
        private ApplicationDbContext _context;
        private IExcelProcessor _excel;

        public VoterManager(ApplicationDbContext context, IExcelProcessor excel)
        {
            _context = context;
            _excel = excel;
        }

        public Operation<VoterModel> CreateVoter(VoterModel model)
        {
            return Operation.Create(() =>
            {
                //model.Validate();
                var isExists = _context.Voters.Where(c => c.VotName== model.VotName).FirstOrDefault();
                if (isExists != null) throw new Exception("voter already exist");

                var entity = model.Create(model);
                _context.Voters.Add(entity);
                _context.SaveChanges();

                return model;
            });
        }
        public Operation<VoterModel[]> GetVoters()
        {
            return Operation.Create(() =>
            {
                var entities = _context.Voters.ToList();

                var models = entities.Select(c => new VoterModel(c)
                {
                    //User = new ApplicationUser(c.User)
                }
                ).ToArray();
                return models;
            });
        }
        public Operation<VoterModel> UpdateVoter(VoterModel model)
        {
            return Operation.Create(() =>
            {
                //model.Validate();
                var isExist = _context.Voters.Find(model.VoterId);
                if (isExist == null) throw new Exception("Voter does not exist");

                var entity = model.Edit(isExist, model);
                _context.Entry(entity);
                _context.SaveChanges();
                return model;
            });
        }
        public Operation<VoterModel> GetVoterById(int voterId)
        {
            return Operation.Create(() =>
            {
                var entity = _context.Voters.Where(c => c.VoterId == voterId).FirstOrDefault();
                if (entity == null) throw new Exception("Voter does not exist");
                return new VoterModel(entity);

            });
        }
     
        public Operation Details(int id)
        {
            return Operation.Create(() =>
            {
                var entity = _context.Voters.Include(s => s.VoterId == id).FirstOrDefault();
                if (entity == null) throw new Exception("Voter does not  exist");
                return new VoterModel(entity);
            });
        }
        public Operation DeleteVoter(int id)
        {
            return Operation.Create(() =>
            {
                var entity = _context.Voters.Find(id);
                if (entity == null) throw new Exception("Voter does not exist");

                _context.Voters.Remove(entity);
                _context.SaveChanges();
            });
        }
        public Operation<List<VoterModel>> UploadVoterNames(Stream stream, VoterModel model)
        {
            return Operation.Create(() =>
            {
                var sheet = _excel.Load<VoterModel>(stream);
                var errors = new List<VoterModel>();
                foreach (var row in sheet)
                {
                    // note: I check if staffNo exist in the database, if null, add the data and save it. if yes, edit the data and save it.
                    var voter = _context.Voters.Where(v => v.StaffNo == row.StaffNo).FirstOrDefault();
                    row.CreatedBy = model.CreatedBy;
                    row.ModifiedBy = model.ModifiedBy;
                    row.CreatedDate = DateTime.Now;
                 
                    if (voter == null)
                    {
                        var voterEntity = new Voter
                        {
                            CreatedBy = row.CreatedBy,
                            ModifiedBy = row.ModifiedBy,
                            CreatedDate = DateTime.Now,


                        VotName = row.VotName,
                            StaffNo = row.StaffNo,
                            //ModifiedDate  = DateTime.Now
                        };
                        _context.Voters.Add(voterEntity);
                        continue;
                    }
            
                    else
                    {
                     
                        voter.StaffNo = row.StaffNo;
                        voter.VotName = row.VotName;
                        voter.CreatedDate = DateTime.Now;
                        //var entity1 = row.Edit(voter, row);
                        _context.Entry(voter);
                     
                    }
                    errors.Add(row); 
                }
                _context.SaveChanges();
                return errors;
            });
        }

        ///////////////// beginning of  meal

        public Operation<MealModel> CreateMeal(MealModel model)
        {
            return Operation.Create(() =>
            {
                //model.Validate();
                var isExists = _context.Meals.Where(c => c.UserId == model.UserId).FirstOrDefault();
                if (isExists != null) throw new Exception("meal already exist");

                var entity = model.Create(model);
                _context.Meals.Add(entity);
                _context.SaveChanges();

                return model;
            });
        }
        public Operation<MealModel[]> GetMeals()
        {
            return Operation.Create(() =>
            {
                var entities = _context.Meals.ToList();

                var models = entities.Select(c => new MealModel(c)
                {
                    //Voters = new VoterModel(c.Voter),
                    Restaurant = new RestaurantModel(c.Restaurant),
                    //Users = new ApplicationUser(c.User) 
                }
                ).ToArray();
                return models;
            });
        }
        public Operation<MealModel> UpdateMeal(MealModel model)
        {
            return Operation.Create(() =>
            {
                //model.Validate();
                var isExist = _context.Meals.Find(model.Id);
                if (isExist == null) throw new Exception("Meal does not exist");

                var entity = model.Edit(isExist, model);
                _context.Entry(entity);
                _context.SaveChanges();
                return model;
            });
        }
        public Operation<MealModel> GetMealById(int mealId)
        {
            return Operation.Create(() =>
            {
                var entity = _context.Meals.Where(c => c.Id == mealId).FirstOrDefault();
                if (entity == null) throw new Exception("Meal does not exist");
                return new MealModel(entity);

            });
        }
        public Operation DeleteMeal(int id)
        {
            return Operation.Create(() =>
            {
                var entity = _context.Meals.Find(id);
                if (entity == null) throw new Exception("Meal does not exist");

                _context.Meals.Remove(entity);
                _context.SaveChanges();
            });
        }
    }
}
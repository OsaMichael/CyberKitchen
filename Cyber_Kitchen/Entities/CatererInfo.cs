﻿using Cyber_Kitchen.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Cyber_Kitchen.Entities
{
    public partial class  CatererInfo
    {
        [Key]
        public int CaterId { get; set; }
        public int? RestId { get; set; }
        public int? VoterId { get; set; }
        public string UserId { get; set; }
        public string CaterName { get; set; }
        
        public int Taste { get; set; }
        
        public int Quality { get; set; }
        
        public int Quantity { get; set; }
        
        public int TimeLiness { get; set; }
        
        public int CustomerServices { get; set; }
       
        public decimal TotalScore { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }

        public virtual Voter Voter { get; set; }
        public virtual Restaurant Restaurant { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}
﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ShopNow.DAL.DomainClasses
{
    public class Store
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [StringLength(250)]
        public string Street { get; set; }
        [StringLength(150)]
        public string City { get; set; }
        [StringLength(5)]
        public string Region { get; set; }
        public double? Longitude { get; set; }
        public double? Latitude { get; set; }
        // distance is a hack to hold output from sproc
        public double? Distance { get; set; }

    }
}

﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphQLDto.Seller
{
    public class Seller_QL
    {
        public long SellerId { get; set; }
        public string SellerName { get; set; }
    }
}
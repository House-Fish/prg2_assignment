﻿//==========================================================
// Student Number : S10256965
// Student Name : Lee Jia Yu
// Partner Name : Yu Yi Lucas
//==========================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S10256965_PRG2Assignment
{
    internal class Flavour
    {
        public string Type { get; set; }
        public bool Premium { get; set; }
        public int Quantity { get; set; }
        public Flavour() { } 
        public Flavour(string type, bool premium, int quantity) 
        { 
            Type = type;
            Premium = premium;
            Quantity = quantity;
        }
        public override string ToString()
        {
            return "flavour"; 
        }
    }
}

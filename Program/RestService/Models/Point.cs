﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RestService.Models
{
    public class Point
    {
        public int Id { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public float Pressure { get; set; }
        public int Timestamp { get; set; }
    }
}
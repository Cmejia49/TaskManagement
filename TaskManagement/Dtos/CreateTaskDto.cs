﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TaskManagement.Dtos
{
    public class CreateTaskDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
    }
}
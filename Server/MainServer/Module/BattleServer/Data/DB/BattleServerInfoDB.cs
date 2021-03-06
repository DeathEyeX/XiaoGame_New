﻿using System;
using System.Collections.Generic;
using System.Text;

namespace RedStone.DB
{
    public class BattleServerInfoDB : CommonDB
    {
        public long uid { get; set; }
        public string deviceID { get; set; }
        public string name { get; set; }
        public int gold { get; set; }
        public int level { get; set; }
        public int exp;
        public bool isOnline { get; set; }
    }
}

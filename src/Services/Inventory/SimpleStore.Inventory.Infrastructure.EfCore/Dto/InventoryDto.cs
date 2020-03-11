﻿using System;

namespace SimpleStore.Inventory.Infrastructure.EfCore.Dto
{
    public class InventoryDto
    {
        public Guid InventoryId { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
    }
}

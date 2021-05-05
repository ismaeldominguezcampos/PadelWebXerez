using System;

namespace PadelWebXerez.Models
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }
        public byte[] Timestamp { get; set; }
    }
}

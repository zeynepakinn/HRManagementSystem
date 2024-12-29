﻿namespace WebApplication1.Models
{
    public class Attendance
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public DateTime CheckInTime { get; set; }
        public DateTime CheckOutTime { get; set; }
        public bool IsAbsent { get; set; }

        // Navigation property
        public Employee Employee { get; set; }
    }
}

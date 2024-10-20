﻿using LibraryWebApp.AuthService.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LibraryWebApp.AuthService.Domain.Entities
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? HashedPassword { get; set; }
        public UserRole Role { get; set; } = UserRole.User;

        public ICollection<Book> Books { get; set; } = new List<Book>();
        public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    }
}

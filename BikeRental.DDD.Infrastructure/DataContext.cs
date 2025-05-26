using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BikeRental.DDD.Domain;
using BikeRental.DDD.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Reflection.Emit;
using BikeRental.DDD.Domain.DTOs.Common;

namespace BikeRental.DDD.Infrastructure
{
    public class DataContext : IdentityDbContext<User, Role, int, IdentityUserClaim<int>, UserRole, IdentityUserLogin<int>, IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        private readonly IPublisher _publisher;
        private ILogger<DataContext> _logger;

        public DataContext(DbContextOptions<DataContext> options,
                IPublisher publisher,
                ILogger<DataContext> logger) : base(options)
        {
            _publisher = publisher;
            _logger = logger;
        }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<Bike> Bikes { get; set; }        
        public DbSet<BikePhoto> BikePhotos { get; set; }
        public DbSet<UserPhoto> UserPhotos { get; set; }
        public DbSet<Like> Likes { get; set; }        
        public DbSet<Message> Messages { get; set; }        
        public DbSet<Rental> Rentals { get; set; }        

        public DbSet<Group> Groups { get; set; }
        public DbSet<Connection> Connections { get; set; }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await base.SaveChangesAsync(cancellationToken);

                var events = ChangeTracker.Entries<IHasDomainEvent>()
                        .Select(x => x.Entity.DomainEvents)
                        .SelectMany(x => x)
                        .Where(domainEvent => !domainEvent.IsPublished)
                        .ToArray();

                foreach (var @event in events)
                {
                    @event.IsPublished = true;

                    _logger.LogInformation("New domain event {Event}", @event.GetType().Name);

                    await _publisher.Publish(@event);
                }

                return result;
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return -1;
            }
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<DomainEvent>().HasNoKey();       

            builder.Entity<User>()
                .Ignore(x => x.DomainEvents);

            builder.Entity<User>()
                .Property(x => x.Id)
                .ValueGeneratedOnAdd();

            builder.Entity<Role>(b =>
            {
                b.Property(r => r.Id)
                 .ValueGeneratedOnAdd();
            });

            builder.Entity<UserPhoto>()
               .Ignore(up => up.DomainEvents);

            builder.Entity<User>(entity =>
            {
                entity.OwnsOne(u => u.Address, address =>
                {
                    address.Property(a => a.Street).HasMaxLength(100);
                    address.Property(a => a.HouseNumber).HasMaxLength(10);
                    address.Property(a => a.Zip).HasMaxLength(10);
                    address.Property(a => a.City).HasMaxLength(50);
                    address.Property(a => a.Country).HasMaxLength(50);
                });
            });

            builder.Entity<User>()
                .HasMany(ur => ur.UserRoles)
                .WithOne(u => u.User)
                .HasForeignKey(ur => ur.UserId)
                .IsRequired();

            builder.Entity<Bike>()
                .Ignore(x => x.DomainEvents)
                .Property(b => b.Price)
                .HasColumnType("decimal(10,2)"); 

            builder.Entity<BikePhoto>()
                .Ignore(x => x.DomainEvents);                     

            builder.Entity<Like>()
                .Ignore(x => x.DomainEvents);
            

            

            builder.Entity<Role>()
                .HasMany(ur => ur.UserRoles)
                .WithOne(u => u.Role)
                .HasForeignKey(ur => ur.RoleId)
                .IsRequired();

            builder.Entity<Rental>()
                .Ignore(x => x.DomainEvents);

            builder.Entity<Rental>()
                .HasOne(r => r.Customer)
                .WithMany(u => u.Rentals)
                .HasForeignKey(r => r.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Rental>()
                .HasOne(r => r.Bike)
                .WithMany(b => b.Rentals)
                .HasForeignKey(r => r.BikeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Like>()
                .HasKey(l => new { l.UserId, l.BikeId });

            builder.Entity<Like>()
                .HasOne(l => l.User)
                .WithMany(u => u.LikedBikes)
                .HasForeignKey(l => l.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Like>()
                .HasOne(l => l.Bike)
                .WithMany(b => b.LikedByUsers)
                .HasForeignKey(l => l.BikeId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Message>()
                .HasOne(u => u.Recipient)
                .WithMany(m => m.MessagesReceived)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Message>()
                .HasOne(u => u.Sender)
                .WithMany(m => m.MessagesSent)
                .OnDelete(DeleteBehavior.Restrict);            
        }
    }
}

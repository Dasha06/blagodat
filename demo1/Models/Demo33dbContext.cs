using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace demo1.Models;

public partial class Demo33dbContext : DbContext
{
    public Demo33dbContext()
    {
    }

    public Demo33dbContext(DbContextOptions<Demo33dbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Client> Clients { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderAndService> OrderAndServices { get; set; }

    public virtual DbSet<Post> Posts { get; set; }

    public virtual DbSet<Service> Services { get; set; }

    public virtual DbSet<Worker> Workers { get; set; }

    public virtual DbSet<WorkerEnterDate> WorkerEnterDates { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Port=5433;Database=demo33db;Username=postgres;Password=123");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.ClientId).HasName("clients_pkey");

            entity.ToTable("clients", "Demo1");

            entity.Property(e => e.ClientId)
                .ValueGeneratedNever()
                .HasColumnName("client_id");
            entity.Property(e => e.ClentPassport).HasColumnName("clent_passport");
            entity.Property(e => e.ClientAddress).HasColumnName("client_address");
            entity.Property(e => e.ClientBirthday).HasColumnName("client_birthday");
            entity.Property(e => e.ClientEmail).HasColumnName("client_email");
            entity.Property(e => e.ClientFirstName).HasColumnName("client_first_name");
            entity.Property(e => e.ClientLastName).HasColumnName("client_last_name");
            entity.Property(e => e.ClientMidName).HasColumnName("client_mid_name");
            entity.Property(e => e.ClientPassword).HasColumnName("client_password");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("orders_pkey");

            entity.ToTable("orders", "Demo1");

            entity.Property(e => e.OrderId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("order_id");
            entity.Property(e => e.OrderClientId).HasColumnName("order_client_id");
            entity.Property(e => e.OrderCreateDate).HasColumnName("order_create_date");
            entity.Property(e => e.OrderCreateTime).HasColumnName("order_create_time");
            entity.Property(e => e.OrderDateFinish).HasColumnName("order_date_finish");
            entity.Property(e => e.OrderRentalTime).HasColumnName("order_rental_time");
            entity.Property(e => e.OrderStatus).HasColumnName("order_status");

            entity.HasOne(d => d.OrderClient).WithMany(p => p.Orders)
                .HasForeignKey(d => d.OrderClientId)
                .HasConstraintName("orders_order_client_id_fkey");
        });

        modelBuilder.Entity<OrderAndService>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("order_and_services", "Demo1");

            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.ServiceId).HasColumnName("service_id");

            entity.HasOne(d => d.Order).WithMany()
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("order_and_services_order_id_fkey");

            entity.HasOne(d => d.Service).WithMany()
                .HasForeignKey(d => d.ServiceId)
                .HasConstraintName("order_and_services_service_id_fkey");
        });

        modelBuilder.Entity<Post>(entity =>
        {
            entity.HasKey(e => e.PostId).HasName("posts_pkey");

            entity.ToTable("posts", "Demo1");

            entity.Property(e => e.PostId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("post_id");
            entity.Property(e => e.PostName).HasColumnName("post_name");
        });

        modelBuilder.Entity<Service>(entity =>
        {
            entity.HasKey(e => e.ServiceId).HasName("services_pkey");

            entity.ToTable("services", "Demo1");

            entity.Property(e => e.ServiceId)
                .ValueGeneratedNever()
                .HasColumnName("service_id");
            entity.Property(e => e.ServiceCode).HasColumnName("service_code");
            entity.Property(e => e.ServiceCostPerHour).HasColumnName("service_cost_per_hour");
            entity.Property(e => e.ServiceName).HasColumnName("service_name");
        });

        modelBuilder.Entity<Worker>(entity =>
        {
            entity.HasKey(e => e.WorkerId).HasName("workers_pkey");

            entity.ToTable("workers", "Demo1");

            entity.Property(e => e.WorkerId)
                .ValueGeneratedNever()
                .HasColumnName("worker_id");
            entity.Property(e => e.WorkerFirstName).HasColumnName("worker_first_name");
            entity.Property(e => e.WorkerLastName).HasColumnName("worker_last_name");
            entity.Property(e => e.WorkerLogin).HasColumnName("worker_login");
            entity.Property(e => e.WorkerMidName).HasColumnName("worker_mid_name");
            entity.Property(e => e.WorkerPassword).HasColumnName("worker_password");
            entity.Property(e => e.WorkerPost).HasColumnName("worker_post");

            entity.HasOne(d => d.WorkerPostNavigation).WithMany(p => p.Workers)
                .HasForeignKey(d => d.WorkerPost)
                .HasConstraintName("workers_worker_post_fkey");
        });

        modelBuilder.Entity<WorkerEnterDate>(entity =>
        {
            entity.HasKey(e => e.EnterId).HasName("worker_enter_dates_pkey");

            entity.ToTable("worker_enter_dates", "Demo1");

            entity.Property(e => e.EnterId)
                .ValueGeneratedNever()
                .HasColumnName("enter_id");
            entity.Property(e => e.WorkerEnterType).HasColumnName("worker_enter_type");
            entity.Property(e => e.WorkerId).HasColumnName("worker_id");
            entity.Property(e => e.WorkerLastEnter)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("worker_last_enter");

            entity.HasOne(d => d.Worker).WithMany(p => p.WorkerEnterDates)
                .HasForeignKey(d => d.WorkerId)
                .HasConstraintName("worker_enter_dates_worker_id_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

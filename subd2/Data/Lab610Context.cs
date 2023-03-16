using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using subd2.Models;

namespace subd2.Data;

public partial class Lab610Context : DbContext
{
    public Lab610Context()
    {
    }

    public Lab610Context(DbContextOptions<Lab610Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Budget> Budgets { get; set; }

    public virtual DbSet<BuyRawMaterial> BuyRawMaterials { get; set; }

    public virtual DbSet<CreateProduct> CreateProducts { get; set; }

    public virtual DbSet<FinishedProduct> FinishedProducts { get; set; }

    public virtual DbSet<GetEmployee> GetEmployees { get; set; }

    public virtual DbSet<GetStuffOfFood> GetStuffOfFoods { get; set; }

    public virtual DbSet<HistoryOfProduct> HistoryOfProducts { get; set; }

    public virtual DbSet<Ingredient> Ingredients { get; set; }

    public virtual DbSet<JobsTitle> JobsTitles { get; set; }

    public virtual DbSet<RawMaterial> RawMaterials { get; set; }

    public virtual DbSet<SellProduct> SellProducts { get; set; }

    public virtual DbSet<Stuff> Stuffs { get; set; }

    public virtual DbSet<Unit> Units { get; set; }

    public virtual DbSet<Recipe> Recipe { get; set; }

    public virtual DbSet<Payments> Payments { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source = DESKTOP-EJJDO3D\\SQLEXPRESS; initial catalog =lab6-10; trusted_connection = yes;TrustServerCertificate=true;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BuyRawMaterial>(entity =>
        {
            entity.ToTable("Buy_raw_materials", tb => tb.HasTrigger("OnByuRaw"));

            entity.HasOne(d => d.EmployeeNavigation).WithMany(p => p.BuyRawMaterials).HasConstraintName("FK_Buy_raw_materials_Stuff");

            entity.HasOne(d => d.RawMaterialsNavigation).WithMany(p => p.BuyRawMaterials).HasConstraintName("FK_Buy_raw_materials_Raw_materials");
        });

        modelBuilder.Entity<CreateProduct>(entity =>
        {
            entity.ToTable("Create_product", tb => tb.HasTrigger("OnCreateProduct"));

            entity.HasOne(d => d.EmployeeNavigation).WithMany(p => p.CreateProducts).HasConstraintName("FK_Create_product_Stuff");

            entity.HasOne(d => d.ProductNavigation).WithMany(p => p.CreateProducts).HasConstraintName("FK_Create_product_Finished_products");
        });

        modelBuilder.Entity<FinishedProduct>(entity =>
        {
            entity.Property(e => e.Cost).HasComputedColumnSql("(case when [Count]=(0) then (0) else round(round([Sum],(2))/round([Count],(2)),(2),(1)) end)", false);
            entity.Property(e => e.Sebes).HasDefaultValueSql("((0))");

            entity.HasOne(d => d.Unit).WithMany(p => p.FinishedProducts).HasConstraintName("FK_Finished_products_Unit");
        });

        modelBuilder.Entity<GetEmployee>(entity =>
        {
            entity.ToView("GetEmployees");
        });

        modelBuilder.Entity<GetStuffOfFood>(entity =>
        {
            entity.ToView("GetStuffOfFood");
        });

        modelBuilder.Entity<HistoryOfProduct>(entity =>
        {
            entity.Property(e => e.Action).IsFixedLength();
            entity.Property(e => e.Employee).IsFixedLength();
            //entity.Property(e => e.Idproduct).ValueGeneratedOnAdd();
            entity.Property(e => e.Product).IsFixedLength();
        });

        modelBuilder.Entity<Ingredient>(entity =>
        {
            entity.HasOne(d => d.NameProductionNavigation).WithMany(p => p.Ingredients).HasConstraintName("FK_Ingredients_Finished_products");

            entity.HasOne(d => d.RawMaterialsNavigation).WithMany(p => p.Ingredients).HasConstraintName("FK_Ingredients_Raw_materials");
        });

        modelBuilder.Entity<RawMaterial>(entity =>
        {
            entity.Property(e => e.Cost).HasComputedColumnSql("(case when [Count]=(0) then (0) else round([Sum]/[Count],(2),(1)) end)", true);

            entity.HasOne(d => d.UnitNavigation).WithMany(p => p.RawMaterials).HasConstraintName("FK_Raw_materials_Unit");
        });

        modelBuilder.Entity<SellProduct>(entity =>
        {
            entity.ToTable("Sell_products", tb => tb.HasTrigger("OnSellProduct"));

            entity.HasOne(d => d.EmployeeNavigation).WithMany(p => p.SellProducts).HasConstraintName("FK_Sell_products_Stuff");

            entity.HasOne(d => d.ProductNavigation).WithMany(p => p.SellProducts).HasConstraintName("FK_Sell_products_Finished_products");
        });

        modelBuilder.Entity<Stuff>(entity =>
        {
            entity.HasOne(d => d.JobTitleNavigation).WithMany(p => p.Stuffs).HasConstraintName("FK_Stuff_JobsTitles");
        });

        modelBuilder.Entity<Payments>(entity => {
            entity.HasOne(d => d.EmployeeNavigation).WithMany(p => p.Payments).HasConstraintName("FK_Payments_Stuffs");
            entity.HasOne(d => d.MonthNavigation).WithMany(p => p.Payments).HasConstraintName("FK_Payments_Month");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);


}

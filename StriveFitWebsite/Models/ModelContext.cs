using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace StriveFitWebsite.Models;

public partial class ModelContext : DbContext
{
    public ModelContext()
    {
    }

    public ModelContext(DbContextOptions<ModelContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Aboutuspage> Aboutuspages { get; set; }

    public virtual DbSet<Contactmessage> Contactmessages { get; set; }

    public virtual DbSet<Contactuspage> Contactuspages { get; set; }

    public virtual DbSet<Homepage> Homepages { get; set; }

    public virtual DbSet<Membershipplan> Membershipplans { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Schedule> Schedules { get; set; }

    public virtual DbSet<Subscription> Subscriptions { get; set; }

    public virtual DbSet<Testimonial> Testimonials { get; set; }

    public virtual DbSet<Trainingtype> Trainingtypes { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Userlogin> Userlogins { get; set; }

    public virtual DbSet<Workoutplan> Workoutplans { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseOracle("DATA SOURCE=localhost:1521;USER ID=C##TahalufP;PASSWORD=Test321;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasDefaultSchema("C##TAHALUFP")
            .UseCollation("USING_NLS_COMP");

        modelBuilder.Entity<Aboutuspage>(entity =>
        {
            entity.HasKey(e => e.Pageid).HasName("SYS_C008619");

            entity.ToTable("ABOUTUSPAGE");

            entity.Property(e => e.Pageid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER(38)")
                .HasColumnName("PAGEID");
            entity.Property(e => e.Descriptions)
                .HasColumnType("CLOB")
                .HasColumnName("DESCRIPTIONS");
            entity.Property(e => e.Imageurl)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("IMAGEURL");
            entity.Property(e => e.Subheading)
                .HasColumnType("CLOB")
                .HasColumnName("SUBHEADING");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("TITLE");
        });

        modelBuilder.Entity<Contactmessage>(entity =>
        {
            entity.HasKey(e => e.Messageid).HasName("SYS_C008610");

            entity.ToTable("CONTACTMESSAGES");

            entity.Property(e => e.Messageid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("MESSAGEID");
            entity.Property(e => e.Message)
                .HasColumnType("CLOB")
                .HasColumnName("MESSAGE");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValueSql("'New'")
                .HasColumnName("STATUS");
            entity.Property(e => e.Submissiondate)
                .HasPrecision(6)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("SUBMISSIONDATE");
            entity.Property(e => e.Useremail)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("USEREMAIL");
            entity.Property(e => e.Userid)
                .HasColumnType("NUMBER")
                .HasColumnName("USERID");
            entity.Property(e => e.Username)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("USERNAME");

            entity.HasOne(d => d.User).WithMany(p => p.Contactmessages)
                .HasForeignKey(d => d.Userid)
                .HasConstraintName("FK_CONTACT_USER");
        });

        modelBuilder.Entity<Contactuspage>(entity =>
        {
            entity.HasKey(e => e.Pageid).HasName("SYS_C008621");

            entity.ToTable("CONTACTUSPAGE");

            entity.Property(e => e.Pageid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER(38)")
                .HasColumnName("PAGEID");
            entity.Property(e => e.Contentvalue)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("CONTENTVALUE");
            entity.Property(e => e.Title)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("TITLE");
        });

        modelBuilder.Entity<Homepage>(entity =>
        {
            entity.HasKey(e => e.Pageid).HasName("SYS_C008617");

            entity.ToTable("HOMEPAGE");

            entity.Property(e => e.Pageid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER(38)")
                .HasColumnName("PAGEID");
            entity.Property(e => e.Descriptions)
                .HasColumnType("CLOB")
                .HasColumnName("DESCRIPTIONS");
            entity.Property(e => e.Details)
                .HasColumnType("CLOB")
                .HasColumnName("DETAILS");
            entity.Property(e => e.Headings)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("HEADINGS");
            entity.Property(e => e.Imageurl)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("IMAGEURL");
            entity.Property(e => e.Sectionname)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("SECTIONNAME");
        });

        modelBuilder.Entity<Membershipplan>(entity =>
        {
            entity.HasKey(e => e.Planid).HasName("SYS_C008518");

            entity.ToTable("MEMBERSHIPPLANS");

            entity.Property(e => e.Planid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER(38)")
                .HasColumnName("PLANID");
            entity.Property(e => e.Details)
                .HasColumnType("CLOB")
                .HasColumnName("DETAILS");
            entity.Property(e => e.Durationmonths)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("DURATIONMONTHS");
            entity.Property(e => e.Planname)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("PLANNAME");
            entity.Property(e => e.Price)
                .HasColumnType("NUMBER(10,2)")
                .HasColumnName("PRICE");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.Notificationid).HasName("SYS_C008538");

            entity.ToTable("NOTIFICATIONS");

            entity.Property(e => e.Notificationid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER(38)")
                .HasColumnName("NOTIFICATIONID");
            entity.Property(e => e.Createddate)
                .HasPrecision(6)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("CREATEDDATE");
            entity.Property(e => e.Isread)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasDefaultValueSql("'N'")
                .IsFixedLength()
                .HasColumnName("ISREAD");
            entity.Property(e => e.Message)
                .HasColumnType("CLOB")
                .HasColumnName("MESSAGE");
            entity.Property(e => e.Userid)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("USERID");

            entity.HasOne(d => d.User).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.Userid)
                .HasConstraintName("FK_USERID");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.Paymentid).HasName("SYS_C008552");

            entity.ToTable("PAYMENTS");

            entity.Property(e => e.Paymentid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER(38)")
                .HasColumnName("PAYMENTID");
            entity.Property(e => e.Amount)
                .HasColumnType("NUMBER(10,2)")
                .HasColumnName("AMOUNT");
            entity.Property(e => e.Paymentdate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("DATE")
                .HasColumnName("PAYMENTDATE");
            entity.Property(e => e.Paymentmethod)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("PAYMENTMETHOD");
            entity.Property(e => e.Paymentstatus)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValueSql("'Completed'")
                .HasColumnName("PAYMENTSTATUS");
            entity.Property(e => e.Subscriptionid)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("SUBSCRIPTIONID");

            entity.HasOne(d => d.Subscription).WithMany(p => p.Payments)
                .HasForeignKey(d => d.Subscriptionid)
                .HasConstraintName("FK_SUBSCRIPTIONID");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Roleid).HasName("SYS_C008546");

            entity.ToTable("ROLE");

            entity.HasIndex(e => e.Rolename, "SYS_C008547").IsUnique();

            entity.Property(e => e.Roleid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER(38)")
                .HasColumnName("ROLEID");
            entity.Property(e => e.Rolename)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("ROLENAME");
        });

        modelBuilder.Entity<Schedule>(entity =>
        {
            entity.HasKey(e => e.Scheduleid).HasName("SYS_C008578");

            entity.ToTable("SCHEDULES");

            entity.Property(e => e.Scheduleid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER(38)")
                .HasColumnName("SCHEDULEID");
            entity.Property(e => e.Capacity)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("CAPACITY");
            entity.Property(e => e.Classtype)
                .HasMaxLength(225)
                .IsUnicode(false)
                .HasDefaultValueSql("'Group'")
                .HasColumnName("CLASSTYPE");
            entity.Property(e => e.Endtime)
                .HasColumnType("DATE")
                .HasColumnName("ENDTIME");
            entity.Property(e => e.Exercisroutine)
                .HasColumnType("CLOB")
                .HasColumnName("EXERCISROUTINE");
            entity.Property(e => e.Goal)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("GOAL");
            entity.Property(e => e.Lectuerstime)
                .HasColumnType("INTERVAL DAY(2) TO SECOND(6)")
                .HasColumnName("LECTUERSTIME");
            entity.Property(e => e.Schedulestatus)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValueSql("'Active'")
                .HasColumnName("SCHEDULESTATUS");
            entity.Property(e => e.Starttime)
                .HasColumnType("DATE")
                .HasColumnName("STARTTIME");
            entity.Property(e => e.Trainerid)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("TRAINERID");
            entity.Property(e => e.Trainingid)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("TRAININGID");

            entity.HasOne(d => d.Trainer).WithMany(p => p.Schedules)
                .HasForeignKey(d => d.Trainerid)
                .HasConstraintName("FK_TRAINER_ID");

            entity.HasOne(d => d.Training).WithMany(p => p.Schedules)
                .HasForeignKey(d => d.Trainingid)
                .HasConstraintName("FK_TRAINING_ID");
        });

        modelBuilder.Entity<Subscription>(entity =>
        {
            entity.HasKey(e => e.Subscriptionid).HasName("SYS_C008524");

            entity.ToTable("SUBSCRIPTIONS");

            entity.Property(e => e.Subscriptionid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER(38)")
                .HasColumnName("SUBSCRIPTIONID");
            entity.Property(e => e.Enddate)
                .HasColumnType("DATE")
                .HasColumnName("ENDDATE");
            entity.Property(e => e.Paymentstatus)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValueSql("'Paid'")
                .HasColumnName("PAYMENTSTATUS");
            entity.Property(e => e.Planid)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("PLANID");
            entity.Property(e => e.Renewalstatus)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValueSql("'Active'")
                .HasColumnName("RENEWALSTATUS");
            entity.Property(e => e.Startdate)
                .HasColumnType("DATE")
                .HasColumnName("STARTDATE");
            entity.Property(e => e.Userid)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("USERID");

            entity.HasOne(d => d.Plan).WithMany(p => p.Subscriptions)
                .HasForeignKey(d => d.Planid)
                .HasConstraintName("FK_PLAN_ID");

            entity.HasOne(d => d.User).WithMany(p => p.Subscriptions)
                .HasForeignKey(d => d.Userid)
                .HasConstraintName("FK_USER_ID");
        });

        modelBuilder.Entity<Testimonial>(entity =>
        {
            entity.HasKey(e => e.Testimonialid).HasName("SYS_C008593");

            entity.ToTable("TESTIMONIALS");

            entity.Property(e => e.Testimonialid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER(38)")
                .HasColumnName("TESTIMONIALID");
            entity.Property(e => e.Content)
                .HasColumnType("CLOB")
                .HasColumnName("CONTENT");
            entity.Property(e => e.Memberid)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("MEMBERID");
            entity.Property(e => e.Rating)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("RATING");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValueSql("'Pending'")
                .HasColumnName("STATUS");
            entity.Property(e => e.Submitteddate)
                .HasPrecision(6)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("SUBMITTEDDATE");

            entity.HasOne(d => d.Member).WithMany(p => p.Testimonials)
                .HasForeignKey(d => d.Memberid)
                .HasConstraintName("FK_MEMBER_ID");
        });

        modelBuilder.Entity<Trainingtype>(entity =>
        {
            entity.HasKey(e => e.Trainingtypeid).HasName("SYS_C008565");

            entity.ToTable("TRAININGTYPES");

            entity.HasIndex(e => e.Trainingtypename, "SYS_C008566").IsUnique();

            entity.Property(e => e.Trainingtypeid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER(38)")
                .HasColumnName("TRAININGTYPEID");
            entity.Property(e => e.Isactive)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasDefaultValueSql("'Y'\n")
                .IsFixedLength()
                .HasColumnName("ISACTIVE");
            entity.Property(e => e.Trainingtypename)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("TRAININGTYPENAME");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Userid).HasName("SYS_C008512");

            entity.ToTable("USERS");

            entity.HasIndex(e => e.Email, "SYS_C008513").IsUnique();

            entity.HasIndex(e => e.Name, "UC_USERNAME").IsUnique();

            entity.Property(e => e.Userid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER(38)")
                .HasColumnName("USERID");
            entity.Property(e => e.Balance)
                .HasDefaultValueSql("0.00")
                .HasColumnType("NUMBER(10,2)")
                .HasColumnName("BALANCE");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("EMAIL");
            entity.Property(e => e.Imagepath)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("IMAGEPATH");
            entity.Property(e => e.Isactive)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasDefaultValueSql("'Y'\n")
                .IsFixedLength()
                .HasColumnName("ISACTIVE");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("NAME");
        });

        modelBuilder.Entity<Userlogin>(entity =>
        {
            entity.HasKey(e => e.Loginid).HasName("SYS_C008559");

            entity.ToTable("USERLOGIN");

            entity.HasIndex(e => e.Username, "SYS_C008560").IsUnique();

            entity.Property(e => e.Loginid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER(38)")
                .HasColumnName("LOGINID");
            entity.Property(e => e.Isactive)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasDefaultValueSql("'Y'")
                .IsFixedLength()
                .HasColumnName("ISACTIVE");
            entity.Property(e => e.Lastlogin)
                .HasColumnType("DATE")
                .HasColumnName("LASTLOGIN");
            entity.Property(e => e.Passwordhash)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("PASSWORDHASH");
            entity.Property(e => e.Roleid)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("ROLEID");
            entity.Property(e => e.Userid)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("USERID");
            entity.Property(e => e.Username)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("USERNAME");

            entity.HasOne(d => d.Role).WithMany(p => p.Userlogins)
                .HasForeignKey(d => d.Roleid)
                .HasConstraintName("FK_ROLEID");

            entity.HasOne(d => d.User).WithMany(p => p.Userlogins)
                .HasForeignKey(d => d.Userid)
                .HasConstraintName("FK_USERSID");
        });

        modelBuilder.Entity<Workoutplan>(entity =>
        {
            entity.HasKey(e => e.Workoutid).HasName("SYS_C008586");

            entity.ToTable("WORKOUTPLANS");

            entity.Property(e => e.Workoutid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER(38)")
                .HasColumnName("WORKOUTID");
            entity.Property(e => e.Memberid)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("MEMBERID");
            entity.Property(e => e.Scheduleid)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("SCHEDULEID");
            entity.Property(e => e.Trainerid)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("TRAINERID");

            entity.HasOne(d => d.Member).WithMany(p => p.WorkoutplanMembers)
                .HasForeignKey(d => d.Memberid)
                .HasConstraintName("FK_MEMBERID");

            entity.HasOne(d => d.Schedule).WithMany(p => p.Workoutplans)
                .HasForeignKey(d => d.Scheduleid)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_SCHEDULE_ID");

            entity.HasOne(d => d.Trainer).WithMany(p => p.WorkoutplanTrainers)
                .HasForeignKey(d => d.Trainerid)
                .HasConstraintName("FKTRAINER_ID");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

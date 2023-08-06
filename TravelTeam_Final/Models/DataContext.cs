using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace TravelTeam_Final.Models;

public partial class DataContext : DbContext
{
    public DataContext()
    {
    }

    public DataContext(DbContextOptions<DataContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Chatmessage> Chatmessages { get; set; }

    public virtual DbSet<Mediamessage> Mediamessages { get; set; }

    public virtual DbSet<Tag> Tags { get; set; }

    public virtual DbSet<Teamrelation> Teamrelations { get; set; }

    public virtual DbSet<Travelteam> Travelteams { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasDefaultSchema("DENG")
            .UseCollation("USING_NLS_COMP");

        modelBuilder.Entity<Chatmessage>(entity =>
        {
            entity.HasKey(e => new { e.PostTime, e.UserId }).HasName("C_PK");

            entity.ToTable("CHATMESSAGE");

            entity.Property(e => e.PostTime)
                .HasColumnType("DATE")
                .HasColumnName("POST_TIME");
            entity.Property(e => e.UserId)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasColumnName("USER_ID");
            entity.Property(e => e.Content)
                .HasMaxLength(64)
                .HasColumnName("CONTENT");
            entity.Property(e => e.TeamId)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasColumnName("TEAM_ID");

            entity.HasOne(d => d.Team).WithMany(p => p.Chatmessages)
                .HasForeignKey(d => d.TeamId)
                .HasConstraintName("C_PK_TEAM");

            entity.HasOne(d => d.User).WithMany(p => p.Chatmessages)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("C_PK_USER");
        });

        modelBuilder.Entity<Mediamessage>(entity =>
        {
            entity.HasKey(e => e.MediaUrl).HasName("M_PK");

            entity.ToTable("MEDIAMESSAGE");

            entity.Property(e => e.MediaUrl)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasColumnName("MEDIA_URL");
            entity.Property(e => e.MediaSta)
                .HasDefaultValueSql("0 ")
                .HasColumnType("NUMBER")
                .HasColumnName("MEDIA_STA");
            entity.Property(e => e.MteamId)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasColumnName("MTEAM_ID");
            entity.Property(e => e.MuserId)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasColumnName("MUSER_ID");
            entity.Property(e => e.PostTime)
                .HasColumnType("DATE")
                .HasColumnName("post_time");

            entity.HasOne(d => d.Mteam).WithMany(p => p.Mediamessages)
                .HasForeignKey(d => d.MteamId)
                .HasConstraintName("M_FK_TEAM");

            entity.HasOne(d => d.Muser).WithMany(p => p.Mediamessages)
                .HasForeignKey(d => d.MuserId)
                .HasConstraintName("M_FK_USER");
        });

        modelBuilder.Entity<Tag>(entity =>
        {
            entity.HasKey(e => e.TagName).HasName("T_PK_TAG_NAME");

            entity.ToTable("TAGS");

            entity.Property(e => e.TagName)
                .HasMaxLength(16)
                .HasColumnName("TAG_NAME");
            entity.Property(e => e.TeamId)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasColumnName("TEAM_ID");

            entity.HasOne(d => d.Team).WithMany(p => p.Tags)
                .HasForeignKey(d => d.TeamId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("T_FK_TEAMID");
        });

        modelBuilder.Entity<Teamrelation>(entity =>
        {
            entity.HasKey(e => new { e.TeamId, e.UserId }).HasName("R_PK_RELATION");

            entity.ToTable("TEAMRELATION");

            entity.Property(e => e.TeamId)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasColumnName("TEAM_ID");
            entity.Property(e => e.UserId)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasColumnName("USER_ID");
            entity.Property(e => e.UserStatus)
                .HasColumnType("NUMBER")
                .HasColumnName("USER_STATUS");

            entity.HasOne(d => d.Team).WithMany(p => p.Teamrelations)
                .HasForeignKey(d => d.TeamId)
                .HasConstraintName("R_FK_TEAM");

            entity.HasOne(d => d.User).WithMany(p => p.Teamrelations)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("R_FK_USER");
        });

        modelBuilder.Entity<Travelteam>(entity =>
        {
            entity.HasKey(e => e.TeamId).HasName("PK_TEAM_ID");

            entity.ToTable("TRAVELTEAMS");

            entity.Property(e => e.TeamId)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasDefaultValueSql("SYS_GUID() ")
                .HasColumnName("TEAM_ID");
            entity.Property(e => e.Arrangement)
                .HasMaxLength(500)
                .HasColumnName("ARRANGEMENT");
            entity.Property(e => e.Currentnumber)
                .HasColumnType("NUMBER")
                .HasColumnName("CURRENTNUMBER");
            entity.Property(e => e.Destination)
                .HasMaxLength(32)
                .HasColumnName("DESTINATION");
            entity.Property(e => e.Maxnumber)
                .HasColumnType("NUMBER")
                .HasColumnName("MAXNUMBER");
            entity.Property(e => e.PostTime)
                .HasColumnType("DATE")
                .HasColumnName("POST_TIME");
            entity.Property(e => e.TeamName)
                .HasMaxLength(64)
                .HasColumnName("TEAM_NAME");
            entity.Property(e => e.TeamStatus)
                .HasColumnType("NUMBER")
                .HasColumnName("TEAM_STATUS");
            entity.Property(e => e.TravelBeginTime)
                .HasColumnType("DATE")
                .HasColumnName("TRAVEL_BEGIN_TIME");
            entity.Property(e => e.TravelEndTime)
                .HasColumnType("DATE")
                .HasColumnName("TRAVEL_END_TIME");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK_USER_ID");

            entity.ToTable("USERS");

            entity.Property(e => e.UserId)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasDefaultValueSql("SYS_GUID()\n")
                .HasColumnName("USER_ID");
            entity.Property(e => e.Birthday)
                .HasColumnType("DATE")
                .HasColumnName("BIRTHDAY");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("EMAIL");
            entity.Property(e => e.Gender)
                .HasColumnType("NUMBER")
                .HasColumnName("GENDER");
            entity.Property(e => e.HeadImageUrl)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("HEAD_IMAGE_URL");
            entity.Property(e => e.Location)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("LOCATION");
            entity.Property(e => e.NickName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("NICK_NAME");
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("PASSWORD");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("PHONE_NUMBER");
            entity.Property(e => e.VipGrade)
                .HasDefaultValueSql("0\n")
                .HasColumnType("NUMBER")
                .HasColumnName("VIP_GRADE");
            entity.Property(e => e.WechatNumber)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("WECHAT_NUMBER");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

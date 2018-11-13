using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using BudgetUnderControl.Domain;
using BudgetUnderControl.Common.Enums;

namespace Migrations.Migrations
{
    [DbContext(typeof(Context))]
    [Migration("20170724151410_CreatedAndModifiedDates")]
    partial class CreatedAndModifiedDates
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2");

            modelBuilder.Entity("BudgetUnderControl.Domain.Account", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccountGroupId");

                    b.Property<string>("Comment");

                    b.Property<int>("CurrencyId");

                    b.Property<bool>("IsIncludedToTotal");

                    b.Property<string>("Name")
                        .HasMaxLength(250);

                    b.Property<int>("Order");

                    b.HasKey("Id");

                    b.HasIndex("AccountGroupId");

                    b.HasIndex("CurrencyId");

                    b.ToTable("Account");
                });

            modelBuilder.Entity("BudgetUnderControl.Domain.AccountGroup", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .HasMaxLength(100);

                    b.HasKey("Id");

                    b.ToTable("AccountGroup");
                });

            modelBuilder.Entity("BudgetUnderControl.Domain.AccountSnapshot", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccountId");

                    b.Property<decimal>("Balance");

                    b.Property<DateTime>("CreatedOn");

                    b.Property<DateTime>("Date");

                    b.Property<int>("LastTransactionId");

                    b.Property<DateTime?>("ModifiedOn");

                    b.Property<int?>("PreviousAccountSnapshotId");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.HasIndex("LastTransactionId");

                    b.HasIndex("PreviousAccountSnapshotId");

                    b.ToTable("AccountSnapshot");
                });

            modelBuilder.Entity("BudgetUnderControl.Domain.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .HasMaxLength(100);

                    b.HasKey("Id");

                    b.ToTable("Category");
                });

            modelBuilder.Entity("BudgetUnderControl.Domain.Currency", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Code")
                        .HasMaxLength(3);

                    b.Property<string>("FullName")
                        .HasMaxLength(250);

                    b.Property<short>("Number");

                    b.Property<string>("Symbol")
                        .HasMaxLength(3);

                    b.HasKey("Id");

                    b.ToTable("Currency");
                });

            modelBuilder.Entity("BudgetUnderControl.Domain.ExchangeRate", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("FromCurrencyId");

                    b.Property<double>("Rate");

                    b.Property<int>("ToCurrencyId");

                    b.HasKey("Id");

                    b.HasIndex("FromCurrencyId");

                    b.HasIndex("ToCurrencyId");

                    b.ToTable("ExchangeRate");
                });

            modelBuilder.Entity("BudgetUnderControl.Domain.File", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("FileName");

                    b.HasKey("Id");

                    b.ToTable("File");
                });

            modelBuilder.Entity("BudgetUnderControl.Domain.Icon", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("FileId");

                    b.HasKey("Id");

                    b.ToTable("Icon");
                });

            modelBuilder.Entity("BudgetUnderControl.Domain.Tag", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .HasMaxLength(100);

                    b.HasKey("Id");

                    b.ToTable("Tag");
                });

            modelBuilder.Entity("BudgetUnderControl.Domain.TagToTransaction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("TagId");

                    b.Property<int>("TransactionId");

                    b.HasKey("Id");

                    b.HasIndex("TagId");

                    b.HasIndex("TransactionId");

                    b.ToTable("TagToTransaction");
                });

            modelBuilder.Entity("BudgetUnderControl.Domain.Transaction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccountId");

                    b.Property<decimal>("Amount");

                    b.Property<int?>("CategoryId");

                    b.Property<string>("Comment");

                    b.Property<DateTime>("CreatedOn");

                    b.Property<DateTime>("Date");

                    b.Property<DateTime?>("ModifiedOn");

                    b.Property<string>("Name");

                    b.Property<byte>("Type");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.HasIndex("CategoryId");

                    b.ToTable("Transaction");
                });

            modelBuilder.Entity("BudgetUnderControl.Domain.Transfer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("FromTransactionId");

                    b.Property<decimal>("Rate");

                    b.Property<int>("ToTransactionId");

                    b.HasKey("Id");

                    b.HasIndex("FromTransactionId");

                    b.HasIndex("ToTransactionId");

                    b.ToTable("Transfer");
                });

            modelBuilder.Entity("BudgetUnderControl.Domain.Account", b =>
                {
                    b.HasOne("BudgetUnderControl.Domain.AccountGroup", "AccountGroup")
                        .WithMany("Accounts")
                        .HasForeignKey("AccountGroupId")
                        .HasConstraintName("ForeignKey_Account_AccountGroup")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("BudgetUnderControl.Domain.Currency", "Currency")
                        .WithMany("Accounts")
                        .HasForeignKey("CurrencyId")
                        .HasConstraintName("ForeignKey_Account_Currency")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BudgetUnderControl.Domain.AccountSnapshot", b =>
                {
                    b.HasOne("BudgetUnderControl.Domain.Account", "Account")
                        .WithMany("AccountSnapshots")
                        .HasForeignKey("AccountId")
                        .HasConstraintName("ForeignKey_AccountSnapshot_Account")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("BudgetUnderControl.Domain.Transaction", "LastTransaction")
                        .WithMany("AccountSnapshots")
                        .HasForeignKey("LastTransactionId")
                        .HasConstraintName("ForeignKey_AccountSnapshot_LastTransaction")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("BudgetUnderControl.Domain.AccountSnapshot", "PreviousAccountSnapshot")
                        .WithMany()
                        .HasForeignKey("PreviousAccountSnapshotId");
                });

            modelBuilder.Entity("BudgetUnderControl.Domain.ExchangeRate", b =>
                {
                    b.HasOne("BudgetUnderControl.Domain.Currency", "FromCurrency")
                        .WithMany("FromExchangeRates")
                        .HasForeignKey("FromCurrencyId")
                        .HasConstraintName("ForeignKey_ExchangeRate_FromCurrency")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("BudgetUnderControl.Domain.Currency", "ToCurrency")
                        .WithMany("ToExchangeRates")
                        .HasForeignKey("ToCurrencyId")
                        .HasConstraintName("ForeignKey_ExchangeRate_ToCurrency")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BudgetUnderControl.Domain.TagToTransaction", b =>
                {
                    b.HasOne("BudgetUnderControl.Domain.Tag", "Tag")
                        .WithMany("TagToTransactions")
                        .HasForeignKey("TagId")
                        .HasConstraintName("ForeignKey_TagToTransaction_Tag")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("BudgetUnderControl.Domain.Transaction", "Transaction")
                        .WithMany("TagsToTransaction")
                        .HasForeignKey("TransactionId")
                        .HasConstraintName("ForeignKey_TagToTransaction_Transaction")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BudgetUnderControl.Domain.Transaction", b =>
                {
                    b.HasOne("BudgetUnderControl.Domain.Account", "Account")
                        .WithMany("Transactions")
                        .HasForeignKey("AccountId")
                        .HasConstraintName("ForeignKey_Transaction_Account")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("BudgetUnderControl.Domain.Category", "Category")
                        .WithMany("Transactions")
                        .HasForeignKey("CategoryId")
                        .HasConstraintName("ForeignKey_Transaction_Category");
                });

            modelBuilder.Entity("BudgetUnderControl.Domain.Transfer", b =>
                {
                    b.HasOne("BudgetUnderControl.Domain.Transaction", "FromTransaction")
                        .WithMany("FromTransfers")
                        .HasForeignKey("FromTransactionId")
                        .HasConstraintName("ForeignKey_Transfer_FromTransaction")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("BudgetUnderControl.Domain.Transaction", "ToTransaction")
                        .WithMany("ToTransfers")
                        .HasForeignKey("ToTransactionId")
                        .HasConstraintName("ForeignKey_Transfer_ToTransaction")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}

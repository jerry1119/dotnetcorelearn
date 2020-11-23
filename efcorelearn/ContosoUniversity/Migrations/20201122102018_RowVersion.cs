using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ContosoUniversity.Migrations
{
    public partial class RowVersion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "Department",
                type: "BLOB",       //blob类型即为数据库中的大型byte(二进制大型对象)
                rowVersion: true,
                nullable: true);
                
            //添加数据库触发器，该触发器在行更新时将 RowVersion 列设置为随机 blob 值。
             migrationBuilder.Sql(
            @"
                UPDATE Department
                SET RowVersion = randomblob(8)
            ");

            migrationBuilder.Sql(
            @"
                CREATE TRIGGER SetRowVersionOnUpdate
                AFTER UPDATE ON Department
                BEGIN
                    UPDATE Department
                    SET RowVersion = randomblob(8)
                    WHERE rowid = NEW.rowid;
                END
            ");

            migrationBuilder.Sql(
            @"
                CREATE TRIGGER SetRowVersionOnInsert
                AFTER INSERT ON Department
                BEGIN
                    UPDATE Department
                    SET RowVersion = randomblob(8)
                    WHERE rowid = NEW.rowid;
                END
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "Department");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eproject3.Migrations
{
    /// <inheritdoc />
    public partial class UpdateBookingAndShowtimeRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_MovieShowtimes_MovieShowtimeShowtimeId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK__Bookings__SeatId__48CFD27E",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK__Bookings__Showti__47DBAE45",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK__Showtimes__Movie__3D5E1FD2",
                table: "Showtimes");

            migrationBuilder.DropForeignKey(
                name: "FK__Showtimes__Theat__3E52440B",
                table: "Showtimes");

            migrationBuilder.DropPrimaryKey(
                name: "PK__Bookings__73951AED77701958",
                table: "Bookings");

            migrationBuilder.DropPrimaryKey(
                name: "PK__Showtime__32D31F2070723BDE",
                table: "Showtimes");

            migrationBuilder.RenameTable(
                name: "Showtimes",
                newName: "Showtime");

            migrationBuilder.RenameColumn(
                name: "MovieShowtimeShowtimeId",
                table: "Bookings",
                newName: "ShowtimeId1");

            migrationBuilder.RenameIndex(
                name: "UQ__Bookings__5B869AD940417E02",
                table: "Bookings",
                newName: "IX_Bookings_QRCode");

            migrationBuilder.RenameIndex(
                name: "IX_Bookings_MovieShowtimeShowtimeId",
                table: "Bookings",
                newName: "IX_Bookings_ShowtimeId1");

            migrationBuilder.RenameIndex(
                name: "IX_Showtimes_TheaterId",
                table: "Showtime",
                newName: "IX_Showtime_TheaterId");

            migrationBuilder.RenameIndex(
                name: "IX_Showtimes_MovieId",
                table: "Showtime",
                newName: "IX_Showtime_MovieId");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "ShowDate",
                table: "MovieShowtimes",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1),
                oldClrType: typeof(DateOnly),
                oldType: "date",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartTime",
                table: "Showtime",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndTime",
                table: "Showtime",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Bookings",
                table: "Bookings",
                column: "BookingId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Showtime",
                table: "Showtime",
                column: "ShowtimeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_MovieShowtimes_ShowtimeId",
                table: "Bookings",
                column: "ShowtimeId",
                principalTable: "MovieShowtimes",
                principalColumn: "ShowtimeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Seats_SeatId",
                table: "Bookings",
                column: "SeatId",
                principalTable: "Seats",
                principalColumn: "SeatId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Showtime_ShowtimeId1",
                table: "Bookings",
                column: "ShowtimeId1",
                principalTable: "Showtime",
                principalColumn: "ShowtimeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Showtime_Movies_MovieId",
                table: "Showtime",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "MovieId");

            migrationBuilder.AddForeignKey(
                name: "FK_Showtime_Theaters_TheaterId",
                table: "Showtime",
                column: "TheaterId",
                principalTable: "Theaters",
                principalColumn: "TheaterId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_MovieShowtimes_ShowtimeId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Seats_SeatId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Showtime_ShowtimeId1",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Showtime_Movies_MovieId",
                table: "Showtime");

            migrationBuilder.DropForeignKey(
                name: "FK_Showtime_Theaters_TheaterId",
                table: "Showtime");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Bookings",
                table: "Bookings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Showtime",
                table: "Showtime");

            migrationBuilder.RenameTable(
                name: "Showtime",
                newName: "Showtimes");

            migrationBuilder.RenameColumn(
                name: "ShowtimeId1",
                table: "Bookings",
                newName: "MovieShowtimeShowtimeId");

            migrationBuilder.RenameIndex(
                name: "IX_Bookings_ShowtimeId1",
                table: "Bookings",
                newName: "IX_Bookings_MovieShowtimeShowtimeId");

            migrationBuilder.RenameIndex(
                name: "IX_Bookings_QRCode",
                table: "Bookings",
                newName: "UQ__Bookings__5B869AD940417E02");

            migrationBuilder.RenameIndex(
                name: "IX_Showtime_TheaterId",
                table: "Showtimes",
                newName: "IX_Showtimes_TheaterId");

            migrationBuilder.RenameIndex(
                name: "IX_Showtime_MovieId",
                table: "Showtimes",
                newName: "IX_Showtimes_MovieId");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "ShowDate",
                table: "MovieShowtimes",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartTime",
                table: "Showtimes",
                type: "datetime",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndTime",
                table: "Showtimes",
                type: "datetime",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddPrimaryKey(
                name: "PK__Bookings__73951AED77701958",
                table: "Bookings",
                column: "BookingId");

            migrationBuilder.AddPrimaryKey(
                name: "PK__Showtime__32D31F2070723BDE",
                table: "Showtimes",
                column: "ShowtimeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_MovieShowtimes_MovieShowtimeShowtimeId",
                table: "Bookings",
                column: "MovieShowtimeShowtimeId",
                principalTable: "MovieShowtimes",
                principalColumn: "ShowtimeId");

            migrationBuilder.AddForeignKey(
                name: "FK__Bookings__SeatId__48CFD27E",
                table: "Bookings",
                column: "SeatId",
                principalTable: "Seats",
                principalColumn: "SeatId");

            migrationBuilder.AddForeignKey(
                name: "FK__Bookings__Showti__47DBAE45",
                table: "Bookings",
                column: "ShowtimeId",
                principalTable: "Showtimes",
                principalColumn: "ShowtimeId");

            migrationBuilder.AddForeignKey(
                name: "FK__Showtimes__Movie__3D5E1FD2",
                table: "Showtimes",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "MovieId");

            migrationBuilder.AddForeignKey(
                name: "FK__Showtimes__Theat__3E52440B",
                table: "Showtimes",
                column: "TheaterId",
                principalTable: "Theaters",
                principalColumn: "TheaterId");
        }
    }
}

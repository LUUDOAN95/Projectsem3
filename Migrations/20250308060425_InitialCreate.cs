using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eproject3.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Feedback",
                columns: table => new
                {
                    FeedbackId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubmissionDate = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Feedback__6A4BEDD6F70CCA68", x => x.FeedbackId);
                });

            migrationBuilder.CreateTable(
                name: "Gallery",
                columns: table => new
                {
                    GalleryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    UploadDate = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Gallery__CF4F7BB5E0A4453A", x => x.GalleryId);
                });

            migrationBuilder.CreateTable(
                name: "Movies",
                columns: table => new
                {
                    MovieId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TrailerUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Duration = table.Column<int>(type: "int", nullable: true),
                    ReleaseDate = table.Column<DateOnly>(type: "date", nullable: true),
                    IsPopular = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    IsHot = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    CoverImage = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Movies__4BD2941AAC042187", x => x.MovieId);
                });

            migrationBuilder.CreateTable(
                name: "Shops",
                columns: table => new
                {
                    ShopId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ShopName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Category = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Shops__67C557C924430769", x => x.ShopId);
                });

            migrationBuilder.CreateTable(
                name: "Theaters",
                columns: table => new
                {
                    TheaterId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TheaterName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SeatCapacity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Theaters__4D68B2190F04751D", x => x.TheaterId);
                });

            migrationBuilder.CreateTable(
                name: "MovieShowtimes",
                columns: table => new
                {
                    ShowtimeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MovieId = table.Column<int>(type: "int", nullable: false),
                    ShowDate = table.Column<DateOnly>(type: "date", nullable: true),
                    StartTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    HallNumber = table.Column<int>(type: "int", nullable: false),
                    TicketPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AvailableSeats = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovieShowtimes", x => x.ShowtimeId);
                    table.ForeignKey(
                        name: "FK_MovieShowtimes_Movies_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movies",
                        principalColumn: "MovieId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Seats",
                columns: table => new
                {
                    SeatId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TheaterId = table.Column<int>(type: "int", nullable: true),
                    SeatCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    IsBooked = table.Column<bool>(type: "bit", nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Seats__311713F336A23474", x => x.SeatId);
                    table.ForeignKey(
                        name: "FK__Seats__TheaterId__4222D4EF",
                        column: x => x.TheaterId,
                        principalTable: "Theaters",
                        principalColumn: "TheaterId");
                });

            migrationBuilder.CreateTable(
                name: "Showtimes",
                columns: table => new
                {
                    ShowtimeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MovieId = table.Column<int>(type: "int", nullable: true),
                    TheaterId = table.Column<int>(type: "int", nullable: true),
                    StartTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Showtime__32D31F2070723BDE", x => x.ShowtimeId);
                    table.ForeignKey(
                        name: "FK__Showtimes__Movie__3D5E1FD2",
                        column: x => x.MovieId,
                        principalTable: "Movies",
                        principalColumn: "MovieId");
                    table.ForeignKey(
                        name: "FK__Showtimes__Theat__3E52440B",
                        column: x => x.TheaterId,
                        principalTable: "Theaters",
                        principalColumn: "TheaterId");
                });

            migrationBuilder.CreateTable(
                name: "Bookings",
                columns: table => new
                {
                    BookingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    ShowtimeId = table.Column<int>(type: "int", nullable: true),
                    SeatId = table.Column<int>(type: "int", nullable: true),
                    CustomerName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    BookingTime = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    ExpiryTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    PaymentStatus = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true, defaultValue: "Pending"),
                    QRCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    MovieShowtimeShowtimeId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Bookings__73951AED77701958", x => x.BookingId);
                    table.ForeignKey(
                        name: "FK_Bookings_MovieShowtimes_MovieShowtimeShowtimeId",
                        column: x => x.MovieShowtimeShowtimeId,
                        principalTable: "MovieShowtimes",
                        principalColumn: "ShowtimeId");
                    table.ForeignKey(
                        name: "FK__Bookings__SeatId__48CFD27E",
                        column: x => x.SeatId,
                        principalTable: "Seats",
                        principalColumn: "SeatId");
                    table.ForeignKey(
                        name: "FK__Bookings__Showti__47DBAE45",
                        column: x => x.ShowtimeId,
                        principalTable: "Showtimes",
                        principalColumn: "ShowtimeId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_MovieShowtimeShowtimeId",
                table: "Bookings",
                column: "MovieShowtimeShowtimeId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_SeatId",
                table: "Bookings",
                column: "SeatId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_ShowtimeId",
                table: "Bookings",
                column: "ShowtimeId");

            migrationBuilder.CreateIndex(
                name: "UQ__Bookings__5B869AD940417E02",
                table: "Bookings",
                column: "QRCode",
                unique: true,
                filter: "[QRCode] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_MovieShowtimes_MovieId",
                table: "MovieShowtimes",
                column: "MovieId");

            migrationBuilder.CreateIndex(
                name: "IX_Seats_TheaterId",
                table: "Seats",
                column: "TheaterId");

            migrationBuilder.CreateIndex(
                name: "UQ__Seats__B0DB9629F23F8AD2",
                table: "Seats",
                column: "SeatCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Showtimes_MovieId",
                table: "Showtimes",
                column: "MovieId");

            migrationBuilder.CreateIndex(
                name: "IX_Showtimes_TheaterId",
                table: "Showtimes",
                column: "TheaterId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bookings");

            migrationBuilder.DropTable(
                name: "Feedback");

            migrationBuilder.DropTable(
                name: "Gallery");

            migrationBuilder.DropTable(
                name: "Shops");

            migrationBuilder.DropTable(
                name: "MovieShowtimes");

            migrationBuilder.DropTable(
                name: "Seats");

            migrationBuilder.DropTable(
                name: "Showtimes");

            migrationBuilder.DropTable(
                name: "Movies");

            migrationBuilder.DropTable(
                name: "Theaters");
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TransportationAttendance.Domain.Entities;

namespace TransportationAttendance.Infrastructure.Persistence;

public static class DataSeeder
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<TransportationDbContext>();

        await SeedDistrictsAsync(context);
        await SeedLocationsAsync(context);
        await SeedRoutesAndBusesAsync(context);
    }

    private static async Task SeedDistrictsAsync(TransportationDbContext context)
    {
        if (await context.Districts.AnyAsync())
            return;

        var districts = new List<District>
        {
            District.Create("العزيزية مسلم"),
            District.Create("م . باقدو"),
            District.Create("م . فهد"),
            District.Create("الكردي"),
            District.Create("السيح"),
            District.Create("الدعيثة أبو مرخة"),
            District.Create("الجرف الغربي"),
            District.Create("الصالحية"),
            District.Create("العنابس"),
            District.Create("القبلتين"),
            District.Create("الغربية"),
            District.Create("الفيصلية"),
            District.Create("العنبرية"),
            District.Create("السقيا"),
            District.Create("المصانع"),
            District.Create("المستراح"),
            District.Create("السحمان"),
            District.Create("الروضة"),
            District.Create("الكويتية"),
            District.Create("العوالي"),
            District.Create("قربان"),
            District.Create("الخاتم"),
            District.Create("البحر"),
            District.Create("الأعمدة"),
            District.Create("شارع فلسطين"),
            District.Create("عروة"),
            District.Create("الوبرة"),
            District.Create("الرانوناء"),
            District.Create("شوران"),
            District.Create("الجرف الشرقي"),
            District.Create("العزيزية البخاري"),
            District.Create("الدعيثة"),
            District.Create("م طيبة"),
            District.Create("الدويمة"),
            District.Create("الظاهرة"),
            District.Create("العصبة"),
            District.Create("السديري"),
            District.Create("المغاربة"),
            District.Create("الروابي"),
            District.Create("مخطط الزهرة"),
            District.Create("مخطط الأمير نايف"),
            District.Create("القصواء"),
            District.Create("النصر"),
            District.Create("الحزام"),
            District.Create("الخليل"),
            District.Create("الشهداء"),
            District.Create("الطيارية"),
            District.Create("واحة السلام"),
            District.Create("الحرس"),
            District.Create("الشافية"),
            District.Create("الجمعة"),
            District.Create("النسيم"),
            District.Create("الأزهري"),
            District.Create("التلال"),
            District.Create("الشبيبة"),
            District.Create("الربوة"),
            District.Create("الخالدية"),
            District.Create("الإسكان الشمالي"),
            District.Create("المبعوث"),
            District.Create("مخطط الأمير تركي المطار"),
            District.Create("المغيسلة"),
            District.Create("الجبور"),
            District.Create("الزاهدية"),
            District.Create("الملك فهد"),
            District.Create("مهزور"),
            District.Create("الإسكان الجنوبي"),
            District.Create("الوعير"),
            District.Create("العريض"),
            District.Create("الناصرية"),
            District.Create("باقدو"),
            District.Create("م فهد"),
            District.Create("الدويخله"),
            District.Create("العزيزية الإمام البخاري"),
            District.Create("ابو مرخة"),
            District.Create("البركة"),
            District.Create("الهجرة"),
            District.Create("ابو بريقاء"),
            District.Create("حي الدفاع"),
            District.Create("شارع الملك عبدالعزيز"),
            District.Create("حي الخاتم"),
            District.Create("بلاد السديري"),
            District.Create("الأصيفرين"),
            District.Create("الإسكان"),
            District.Create("جمل الليل"),
            District.Create("سلطانة"),
            District.Create("العنابس (سكن الطلاب)"),
            District.Create("مخطط طيبة"),
            District.Create("العزيزية الإمام مسلم")
        };

        await context.Districts.AddRangeAsync(districts);
        await context.SaveChangesAsync();
    }

    private static async Task SeedLocationsAsync(TransportationDbContext context)
    {
        if (await context.Locations.AnyAsync())
            return;

        var locations = new List<Location>
        {
            Location.Create("A8", "موقف A8", "Parking"),
            Location.Create("A9", "موقف A9", "Parking"),
            Location.Create("A10", "موقف A10", "Parking"),
            Location.Create("A11", "موقف A11", "Parking"),
            Location.Create("B8", "موقف B8", "Parking"),
            Location.Create("B9", "موقف B9", "Parking"),
            Location.Create("B10", "موقف B10", "Parking"),
            Location.Create("B11", "موقف B11", "Parking")
        };

        await context.Locations.AddRangeAsync(locations);
        await context.SaveChangesAsync();
    }

    private static async Task SeedRoutesAndBusesAsync(TransportationDbContext context)
    {
        if (await context.Buses.AnyAsync())
            return;

        // ASR Period (Period ID = 3) Buses
        var asrBuses = new[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10",
            "11", "12", "13", "14", "15", "16", "17", "18", "19", "20",
            "21", "22", "23", "24", "25", "26", "27", "28", "29", "30",
            "31", "32", "33", "41", "42", "43", "44", "45", "46", "47",
            "48", "49", "50", "51", "52", "53", "54", "55", "56", "57",
            "58", "59", "60", "61", "62", "63", "64", "65", "66", "67",
            "68", "69", "70", "71", "72", "73", "74", "91", "94", "408", "410" };

        foreach (var busNum in asrBuses)
        {
            var bus = Bus.Create(busNum, $"ASR-{busNum}", 30);
            await context.Buses.AddAsync(bus);
        }

        // MAGHRIB Period (Period ID = 4) Buses
        var maghribBuses = new[] { "301", "302", "303", "304", "305", "306", "307", "308", "309", "310",
            "311", "312", "313", "401", "402", "403", "404", "405", "406", "407", "409" };

        foreach (var busNum in maghribBuses)
        {
            var bus = Bus.Create(busNum, $"MAG-{busNum}", 30);
            await context.Buses.AddAsync(bus);
        }

        // FAJR_DUHA Period (Period ID = 1) Buses
        var fajrBuses = new[] { "101", "102", "103", "104", "105", "106",
            "201", "202", "203", "204", "205", "206", "207", "208", "209", "210",
            "211", "212", "213" };

        foreach (var busNum in fajrBuses)
        {
            var bus = Bus.Create(busNum, $"FJR-{busNum}", 30);
            await context.Buses.AddAsync(bus);
        }

        await context.SaveChangesAsync();
    }
}

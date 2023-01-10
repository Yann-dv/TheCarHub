namespace theCarHub.Data;

public class CarSeed
{
    public static async Task SeedCarsAsync(IApplicationBuilder applicationBuilder)
    {
        using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
        {
            var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
            await context.Database.EnsureCreatedAsync();

            if (!context.Cars.Any())
            {
                // Cars to seed if existing but Empty db
                var carsToSeed = new List<Car>()
                {
                    new()
                    {
                        Year = new DateTime(1991, 01, 01, 00, 00, 00, 0000000),
                        Make = "Mazda",
                        Model = "Miata",
                        Trim = "LE",
                        PurchaseDate = new DateTime(2022, 07, 01, 00, 00, 00, 0000000),
                        PurchasePrice = 1800.00f,
                        Repairs = "Full restoration",
                        RepairCost = 7600.00f,
                        LotDate = new DateTime(2022, 07, 04, 00, 00, 00, 0000000),
                        SellingPrice = 9900.00f,
                        SaleDate = new DateTime(2022, 08, 04, 00, 00, 00, 0000000),
                        Description = "Fully restorated Miata Mazda",
                        ToSale = false
                    },
                    new()
                    {
                        Year = new DateTime(2007, 01, 01, 00, 00, 00, 0000000),
                        Make = "Jeep",
                        Model = "Liberty",
                        Trim = "Sport",
                        PurchaseDate = new DateTime(2022, 02, 04, 00, 00, 00, 0000000),
                        PurchasePrice = 4500.00f,
                        Repairs = "Front wheel bearings",
                        RepairCost = 350.00f,
                        LotDate = new DateTime(2022, 07, 04, 00, 00, 00, 0000000),
                        SellingPrice = 5350.00f,
                        SaleDate = new DateTime(2022, 09, 04, 00, 00, 00, 0000000),
                        Description = "Super jeep",
                        ToSale = false
                    },
                    new()
                    {
                        Year = new DateTime(2007, 01, 01, 00, 00, 00, 0000000),
                        Make = "Dodge",
                        Model = "Grand Caravan",
                        Trim = "Sport",
                        PurchaseDate = new DateTime(2022, 04, 04, 00, 00, 00, 0000000),
                        PurchasePrice = 1800.00f,
                        Repairs = "Radiator, brakes",
                        RepairCost = 690.00f,
                        LotDate = new DateTime(2022, 08, 04, 00, 00, 00, 0000000),
                        SellingPrice = 2990.00f,
                        SaleDate = DateTime.MinValue,
                        Description = "Perfect for travellers",
                        ToSale = false
                    },
                    new()
                    {
                        Year = new DateTime(2017, 01, 01, 00, 00, 00, 0000000),
                        Make = "Ford",
                        Model = "XLT",
                        Trim = "LE",
                        PurchaseDate = new DateTime(2022, 05, 04, 00, 00, 00, 0000000),
                        PurchasePrice = 24350.00f,
                        Repairs = "Tires, brakes",
                        RepairCost = 1100.00f,
                        LotDate = new DateTime(2022, 09, 04, 00, 00, 00, 0000000),
                        SellingPrice = 25950.00f,
                        SaleDate = DateTime.MinValue,
                        Description = "Luxury Ford car",
                        ToSale = false
                    },
                    new()
                    {
                        Year = new DateTime(2008, 01, 01, 00, 00, 00, 0000000),
                        Make = "Honda",
                        Model = "Civic",
                        Trim = "LX",
                        PurchaseDate = new DateTime(2022, 06, 04, 00, 00, 00, 0000000),
                        PurchasePrice = 4000.00f,
                        Repairs = "AC, brakes",
                        RepairCost = 475.00f,
                        LotDate = new DateTime(2022, 09, 04, 00, 00, 00, 0000000),
                        SellingPrice = 4975.00f,
                        SaleDate = new DateTime(2022, 9, 03, 00, 00, 00, 0000000),
                        Description = "Japanese super car",
                        ToSale = false
                    },
                    new()
                    {
                        Year = new DateTime(2016, 01, 01, 00, 00, 00, 0000000),
                        Make = "Volkswagen",
                        Model = "GTI",
                        Trim = "S",
                        PurchaseDate = new DateTime(2022, 06, 04, 00, 00, 00, 0000000),
                        PurchasePrice = 15250.00f,
                        Repairs = "Tires",
                        RepairCost = 440.00f,
                        LotDate = new DateTime(2022, 10, 04, 00, 00, 00, 0000000),
                        SellingPrice = 16190.00f,
                        SaleDate = DateTime.MinValue,
                        Description = "Deutch qualitat",
                        ToSale = false
                    },
                    new()
                    {
                        Year = new DateTime(2013, 01, 01, 00, 00, 00, 0000000),
                        Make = "Ford",
                        Model = "Edge",
                        Trim = "SEL",
                        PurchaseDate = new DateTime(2022, 07, 04, 00, 00, 00, 0000000),
                        PurchasePrice = 10990.00f,
                        Repairs = "Tires, brakes, AC",
                        RepairCost = 950.00f,
                        LotDate = new DateTime(2022, 11, 04, 00, 00, 00, 0000000),
                        SellingPrice = 12440.00f,
                        SaleDate = new DateTime(2023, 01, 01, 00, 00, 00, 0000000),
                        Description = "For the edgers",
                        ToSale = false
                    }
                };

                for (int i = 0; i < carsToSeed.Count; i++)
                {
                    context.Cars.Add(carsToSeed[i]);
                }

                await context.SaveChangesAsync();
            }
        }
    }
}
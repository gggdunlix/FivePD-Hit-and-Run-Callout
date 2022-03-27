using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CitizenFX.Core;
using FivePD.API;
using FivePD.API.Utils;
using CitizenFX.Core.Native;



[CalloutProperties("Hit and Run (Normal)", "GGGDunlix", "0.3.0")]
public class HitAndRunNormal : FivePD.API.Callout
{
    private Ped driver1, driver2;
    private Vehicle car1, car2;
    public HitAndRunNormal()
    {

        InitInfo(World.GetNextPositionOnStreet(Vector3Extension.Around(Game.PlayerPed.Position, 400)));

        ShortName = "Hit and Run";
        CalloutDescription = "A vehicle collision has occured, and one of the drivers fleed. Respond in code 2. ";
        ResponseCode = 2;
        StartDistance = 150f;
    }

    public override async Task OnAccept()
    {
        InitBlip(25);

        var cars = new[]
          {
               VehicleHash.Adder,
               VehicleHash.CarbonRS,
               VehicleHash.Oracle,
               VehicleHash.Oracle2,
               VehicleHash.Phoenix,
               VehicleHash.Vigero,
               VehicleHash.Zentorno,
               VehicleHash.Youga2,
               VehicleHash.Youga,
               VehicleHash.Sultan,
               VehicleHash.SultanRS,
               VehicleHash.Sentinel,
               VehicleHash.Sentinel2,
               VehicleHash.Ruiner,
               VehicleHash.Ruiner2,
               VehicleHash.Ruiner3,
               VehicleHash.Burrito,
               VehicleHash.Burrito2,
               VehicleHash.Burrito3,
               VehicleHash.GBurrito,
               VehicleHash.Bagger,
               VehicleHash.Buffalo,
               VehicleHash.Buffalo2,
               VehicleHash.Comet2,
               VehicleHash.Comet3,
               VehicleHash.Felon,
           };

        car1 = await SpawnVehicle(cars[RandomUtils.Random.Next(cars.Length)], Location, 180);
        car2 = await SpawnVehicle(cars[RandomUtils.Random.Next(cars.Length)], Location);
        car1.Deform(Location, 10000, 100);
        
        car1.EngineHealth = 5;
        
        car1.BodyHealth = 1;
        car2.BodyHealth = 2;

        API.Wait(2);

        driver1 = await SpawnPed(RandomUtils.GetRandomPed(), Location + 5);
        driver2 = await SpawnPed(RandomUtils.GetRandomPed(), Location + 6, 180);

        driver1.AlwaysKeepTask = true;
        driver1.BlockPermanentEvents = true;

        driver2.AlwaysKeepTask = true;
        driver2.BlockPermanentEvents = true;

        driver2.SetIntoVehicle(car2, VehicleSeat.Driver);

        Utilities.ExcludeVehicleFromTrafficStop(car1.NetworkId, true);
        Utilities.ExcludeVehicleFromTrafficStop(car2.NetworkId, true);
    }

    public override void OnStart(Ped player)
    {
        base.OnStart(player);

        car1.Deform(Location, 10000, 100);
        car2.Deform(Location, 10000, 100);
        driver1.AttachBlip();
        car1.AttachBlip();

        driver2.Task.FleeFrom(player);

    }
}
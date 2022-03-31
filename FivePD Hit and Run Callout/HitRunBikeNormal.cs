using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CitizenFX.Core;
using FivePD.API;
using FivePD.API.Utils;
using CitizenFX.Core.Native;



[CalloutProperties("Hit and Run Bicycle (Normal)", "GGGDunlix", "1.3.4")]
public class HitAndRunBikeNormal : FivePD.API.Callout
{
    private Ped biker, driver2;
    private Vehicle bike, car2;
    public HitAndRunBikeNormal()
    {
        Random random = new Random();
        int x = random.Next(1, 100 + 1);
        if (x <= 50)
        {
            InitInfo(World.GetNextPositionOnStreet(Vector3Extension.Around(Game.PlayerPed.Position, 400)));
        }
        else
        {
            InitInfo(World.GetNextPositionOnSidewalk(Vector3Extension.Around(Game.PlayerPed.Position, 400)));
        }
        

        ShortName = "Hit and Run Bicycle Accident";
        CalloutDescription = "A Car has struck a bicycle and fleed. Respond in Code 2 High.";
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
               VehicleHash.Stanier,
               VehicleHash.Superd,
               VehicleHash.Tailgater,
               VehicleHash.Warrener,
               VehicleHash.Stratum,
               VehicleHash.Washington,
               VehicleHash.Surge,
               VehicleHash.Baller,
               VehicleHash.Baller2,
               VehicleHash.Baller4,
               VehicleHash.Baller6,
               VehicleHash.BJXL,
               VehicleHash.Cavalcade,
               VehicleHash.Cavalcade2,
               VehicleHash.Granger,
               VehicleHash.Gresley,
               VehicleHash.Huntley,
               VehicleHash.Habanero,
               VehicleHash.Mesa,
               VehicleHash.Felon,
               VehicleHash.Felon2,
               VehicleHash.Zion,
               VehicleHash.Zion2,
               VehicleHash.Windsor,
               VehicleHash.Windsor2,
               VehicleHash.Buccaneer,
               VehicleHash.Buccaneer2,
               VehicleHash.Dominator,
               VehicleHash.Faction,
               VehicleHash.Faction2,
               VehicleHash.Faction3,
               VehicleHash.Gauntlet,
               VehicleHash.Gauntlet2,

               };
        var bikes = new[]
          {
               VehicleHash.TriBike,
               VehicleHash.TriBike2,
               VehicleHash.TriBike3,
           };
        var bikers = new[]
          {
            PedHash.Cyclist01,
            PedHash.Cyclist01AMY,
           };

        bike = await SpawnVehicle(bikes[RandomUtils.Random.Next(bikes.Length)], Location, 180);
        car2 = await SpawnVehicle(cars[RandomUtils.Random.Next(cars.Length)], Location + 2);
        bike.Deform(Location, 10000, 100);

        bike.EngineHealth = 5;
        
        bike.BodyHealth = 1;
        car2.BodyHealth = 2;

        API.Wait(2);

        biker = await SpawnPed(bikers[RandomUtils.Random.Next(bikers.Length)], Location + 5);
        driver2 = await SpawnPed(RandomUtils.GetRandomPed(), Location + 6, 180);

        biker.AlwaysKeepTask = true;
        biker.BlockPermanentEvents = true;

        driver2.AlwaysKeepTask = true;
        driver2.BlockPermanentEvents = true;

        driver2.SetIntoVehicle(car2, VehicleSeat.Driver);

        Utilities.ExcludeVehicleFromTrafficStop(bike.NetworkId, true);
        Utilities.ExcludeVehicleFromTrafficStop(car2.NetworkId, true);

        PlayerData playerData = Utilities.GetPlayerData();
        VehicleData datacar = await Utilities.GetVehicleData(car2.NetworkId);
        string CallSign = playerData.Callsign;
        string vehicleName = datacar.Name;
        string carColor = datacar.Color;
        ShowNetworkedNotification("~b~" + CallSign + ",~y~ the suspect is driving a " + carColor + " " + vehicleName + ".", "CHAR_CALL911", "CHAR_CALL911", "Dispatch", "Pursuit", 15f);
    }

    public override void OnStart(Ped player)
    {
        base.OnStart(player);

        bike.Deform(Location, 10000, 100);
        car2.Deform(Location, 10000, 100);
        biker.AttachBlip();
        bike.AttachBlip();

        driver2.Task.FleeFrom(player);
        driver2.DrivingStyle = DrivingStyle.Rushed;

    }
}
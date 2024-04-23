using Buccaneer.Models;
using Buccaneer.Models.DTOs;


var builder = WebApplication.CreateBuilder(args);

List<Follower> followers = new List<Follower>
{
    new Follower
    {
        Id = 1,
        PirateId = 3,
        FollowerId = 1,
    },
    new Follower
    {
        Id = 2,
        PirateId = 2,
        FollowerId = 1,
    },
    new Follower
    {
        Id = 3,
        PirateId = 1,
        FollowerId = 2,
    }
};

List<Pirate> pirates = new List<Pirate>
{
    new Pirate
    {
        Id = 1,
        Name = "Captain Barnacle Beard",
        Age = 69,
        Nationality = "The Swashbuckling Scots",
        Rank = "Captain of the Codswallop",
        Ship = "The Scurvy Scallywag",
        ImageUrl =
            "https://images.fineartamerica.com/images/artworkimages/mediumlarge/3/painty-the-pirate-william-gerard.jpg"
    },
    new Pirate
    {
        Id = 2,
        Name = "Pegleg Pete the Parrot Whisperer",
        Age = 420,
        Nationality = "The Marauding Matadors",
        Rank = "First Mate of Mischief",
        Ship = "The Jolly Rogerous",
        ImageUrl =
            "https://play-lh.googleusercontent.com/1JmaNos2Qujzq5nOuUQtDmHK3rL-_1YhMuPOIYAbeIGA5-3FPQ-rvYQGkvETbWUPlIU"
    },
    new Pirate
    {
        Id = 3,
        Name = "Squid-Eye Jack Sparrow Jr.",
        Age = 69420,
        Nationality = "The Rum-Swigging Russians",
        Rank = "Quartermaster of Quirkiness",
        Ship = "The Plundering Puffin",
        ImageUrl =
            "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSw7EENWb58bGv-T4d1xzULdmG7vEtts08cExUMvWBeWWa2uUsMZWh-bSzQ3JnuEVELxb0&usqp=CAU"
    },
    new Pirate
    {
        Id = 4,
        Name = "Swashbucklin' Sally Seadog",
        Age = 42069,
        Nationality = "The Peg-Legged Poles",
        Rank = "Boatswain of Banter",
        Ship = "The Booty Buccaneer",
        ImageUrl =
            "https://play-lh.googleusercontent.com/U70Faml_BVkRi_1V5c4jJNj5It1Oyu2zMRZg99rVVxy248Jlwe4Qohd1v5RiG_19kpMf=w240-h480-rw"
    }
};

List<Story> stories = new List<Story>
{
    new Story
    {
        Id = 1,
        PirateId = 1,
        Title = "The Ghost Ship",
        Content =
            "The crew of the merchant ship Mary Celeste were found mysteriously missing, leaving the ship and its valuable cargo untouched. It's been said that the ship still sails the seas, haunting those who cross its path.",
        Date = new DateTime(1500, 11, 25)
    },
    new Story
    {
        Id = 2,
        PirateId = 2,
        Title = "The Kraken",
        Content =
            "The Kraken, a massive sea monster, has been the subject of many pirate tales. Its tentacles can stretch for miles and it can easily capsize even the largest ships. Many pirates have met their end at the hands of this fearsome creature.",
        Date = new DateTime(1501, 11, 25)
    },
    new Story
    {
        Id = 3,
        PirateId = 3,
        Title = "The Curse of the Flying Dutchman",
        Content =
            "Legend had it that the Flying Dutchman was cursed to sail the seas forever, its crew doomed to an eternal existence as undead pirates. But when a group of adventurers stumbled upon the ship one stormy night, they found that the curse was all too real. Now they must find a way to break the curse before it's too late.",
        Date = new DateTime(1502, 11, 25)
    },
    new Story
    {
        Id = 4,
        PirateId = 4,
        Title = "The Battle of Blackbeard's Bay",
        Content =
            "It was a fierce battle that raged on for hours. The sound of cannons and the clash of swords echoed across the bay. The pirates fought with all their might, determined to come out on top. In the end, it was Blackbeard's crew that emerged victorious, with a chest full of treasure to show for it.",
        Date = new DateTime(1503, 11, 25)
    },
    new Story
    {
        Id = 5,
        PirateId = 5,
        Title = "The Curse of the Kraken",
        Content =
            "Legend had it that the Kraken would rise from the depths of the ocean to claim any ship that sailed too close to its lair. The crew of the Black Pearl had heard the tales, but they didn't believe them. That was until they saw the monstrous creature rise from the waves, its tentacles reaching out to grab them. They fought with all their might, but in the end, only a few managed to escape with their lives.",
        Date = new DateTime(1504, 11, 25)
    }
};

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//get favorite pirates (expand pirate)
app.MapGet(
    "/followers",
    (int? followerId) =>
    {
        List<Follower> returningFollowers = followers;
        if (followerId != null)
        {
            returningFollowers = returningFollowers
                .Where(follower => follower.FollowerId == followerId)
                .ToList();

            if (returningFollowers.Count == 0)
            {
                return Results.BadRequest();
            }
        }
        return Results.Ok(
            returningFollowers.Select(follower =>
            {
                Pirate pirate = pirates.FirstOrDefault(pirate => pirate.Id == follower.PirateId);
                return new GetFollowerDTO
                {
                    Id = follower.Id,
                    PirateId = follower.PirateId,
                    FollowerId = follower.FollowerId,
                    Pirate = new GetFollowerPirateDTO
                    {
                        Id = pirate.Id,
                        Name = pirate.Name,
                        Age = pirate.Age,
                        Nationality = pirate.Nationality,
                        Rank = pirate.Rank,
                        Ship = pirate.Ship,
                        ImageUrl = pirate.ImageUrl
                    }
                };
            })
        );
    }
);

//get pirate by id
//get pirate with name and ship
app.MapGet(
    "/pirates",
    (string? name, string? ship) =>
    {
        List<Pirate> returnPirates = pirates;

        if (name != null)
        {
            returnPirates = returnPirates.Where(pirate => pirate.Name == name).ToList();
        }

        if (ship != null)
        {
            returnPirates = returnPirates.Where(pirate => pirate.Ship == ship).ToList();
        }

        if (returnPirates.Count == 0)
        {
            return Results.NotFound();
        }

        return Results.Ok(
            returnPirates.Select(pirate => new GetPirateDTO
            {
                Id = pirate.Id,
                Name = pirate.Name,
                Age = pirate.Age,
                Nationality = pirate.Nationality,
                Rank = pirate.Rank,
                Ship = pirate.Ship,
                ImageUrl = pirate.ImageUrl,
            })
        );
    }
);

//get stories (expand pirate)
//get follower by follower id and pirate id
//post follower
//delete follower

app.MapGet(
    "/pirates/{id}",
    (int id) =>
    {
        Pirate pirate = pirates.FirstOrDefault(p => p.Id == id);

        if (pirate == null)
        {
            return Results.BadRequest("Pirate ID is null");
        }

        return Results.Ok(
            new PirateDTO
            {
                Id = pirate.Id,
                Name = pirate.Name,
                Age = pirate.Age,
                Nationality = pirate.Nationality,
                Rank = pirate.Rank,
                Ship = pirate.Ship,
                ImageUrl = pirate.ImageUrl
            }
        );
    }
);

app.MapGet(
    "/stories",
    () =>
    {
        return stories.Select(s => new StoryDTO
        {
            Id = s.Id,
            PirateId = s.PirateId,
            Title = s.Title,
            Content = s.Content,
        });
    }
);

app.MapGet(
    "/followers{PirateId}/{FollowerId}", (int? FollowerId, int? PirateId) =>
    {
    
        List<Follower> returnedFollowers = followers;

        if (FollowerId != null)
        {
            returnedFollowers = returnedFollowers.Where(follower => follower.FollowerId == FollowerId).ToList();
        }
        if (PirateId != null)
        {
            returnedFollowers = returnedFollowers.Where(follower => follower.PirateId == PirateId).ToList();
        }
        return
            returnedFollowers.Select(follower => new GetFollowerDTO
            {
                Id = follower.Id,
                PirateId = follower.PirateId,
                FollowerId = follower.FollowerId
            });
        
    }
);


app.MapPost(
    "/followers",
    (PostFollowerDTO follower) =>
    {
        //gets the associated follower and followee to ensure they exist. If not, return bad request
        Pirate followerPirate = pirates.FirstOrDefault(pirate => pirate.Id == follower.FollowerId);
        Pirate followingPirate = pirates.FirstOrDefault(pirate => pirate.Id == follower.PirateId);

        if (followerPirate == null || followingPirate == null)
        {
            return Results.BadRequest();
        }

        Follower newFollower = new Follower
        {
            Id = followers.Max(follower => follower.Id) + 1,
            PirateId = follower.PirateId,
            FollowerId = follower.FollowerId
        };

        followers.Add(newFollower);

        return Results.Ok(
            new GetFollowerDTO
            {
                Id = newFollower.Id,
                PirateId = newFollower.PirateId,
                FollowerId = newFollower.FollowerId
            }
        );
    }
);

app.Run();

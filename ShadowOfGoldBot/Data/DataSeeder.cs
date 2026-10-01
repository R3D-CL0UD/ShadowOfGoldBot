using Microsoft.EntityFrameworkCore;
using ShadowOfGoldBot.Models;

namespace ShadowOfGoldBot.Data;

public static class DataSeeder
{
    public static async Task SeedAsync(AppDbContext db)
    {
        if (await db.Scenes.AnyAsync()) return;

        // ===== SCENE 1 =====
        var scene1 = new Scene
        {
            Id = 1,
            Title = "Scene 1: A Normal Morning",
            Description = @"Canyon Creek, 1887.

The sun bleeds orange over the rocky ridge to the east. Dust hangs in the air, golden and slow. You unlock the door of your general store.

Through the window, you watch the town wake. A farmer loads hay onto a wagon. A preacher rings the church bell.

The door creaks open. A young boy enters. His clothes are torn. He stares at the chocolate on the counter. His hands are shaking.",
            IsEnding = false
        };
        scene1.Choices = new List<Choice>
        {
            new() { Text = "Give him a chocolate for free", NextSceneId = 2, FlagToSet = "HelpedBoy", ReputationTownChange = 1 },
            new() { Text = "Tell him to bring money next time", NextSceneId = 2, FlagToSet = "RefusedBoy", ReputationTownChange = -1 },
            new() { Text = "Give him half a chocolate", NextSceneId = 2, FlagToSet = "HalfHelpedBoy" },
            new() { Text = "Ignore him", NextSceneId = 2, FlagToSet = "IgnoredBoy", ReputationTownChange = -1 },
            new() { Text = "Ask him where his parents are", NextSceneId = 2, FlagToSet = "AskedAboutBoy" }
        };
        db.Scenes.Add(scene1);

        // ===== SCENE 2 =====
        var scene2 = new Scene
        {
            Id = 2,
            Title = "Scene 2: The Old Woman",
            Description = @"The boy leaves. The bell above the door rings. Mrs. Hartley enters.

She is seventy if she is a day. Her eyes are sharp as a hawk's.

""I need two yards of that blue cloth. And don't you dare charge me full price, Elias.""

She leans on the counter. Her knuckles are white.",
            IsEnding = false
        };
        scene2.Choices = new List<Choice>
        {
            new() { Text = "Give her the cloth at full price", NextSceneId = 3, FlagToSet = "FullPriceHartley", ReputationTownChange = -1 },
            new() { Text = "Give her a small discount", NextSceneId = 3, FlagToSet = "DiscountedHartley", ReputationTownChange = 1 },
            new() { Text = "Refuse to lower the price", NextSceneId = 3, FlagToSet = "StubbornHartley", ReputationTownChange = -1 },
            new() { Text = "Ask her about the rumors in town", NextSceneId = 3, FlagToSet = "AskedRumors" },
            new() { Text = "Tell her about the boy", NextSceneId = 3, FlagToSet = "MentionedBoy" }
        };
        db.Scenes.Add(scene2);

        // ===== SCENE 3 =====
        var scene3 = new Scene
        {
            Id = 3,
            Title = "Scene 3: The Farmer",
            Description = @"Mrs. Hartley leaves. A farmer named Thomas enters.

He grows corn on the eastern edge of town. His hands are always rough. Today, his hands are shaking.

""Give me all the ammunition you have. And... do you have a rifle for sale? A good one.""

Outside, the sun climbs higher. But Thomas is still shivering.",
            IsEnding = false
        };
        scene3.Choices = new List<Choice>
        {
            new() { Text = "Sell him everything he asks for", NextSceneId = 4, FlagToSet = "SoldAmmoToThomas", ReputationTownChange = 1 },
            new() { Text = "Ask him what's wrong", NextSceneId = 4, FlagToSet = "AskedThomas" },
            new() { Text = "Refuse to sell and ask him to calm down", NextSceneId = 4, FlagToSet = "RefusedThomas", ReputationTownChange = -1 },
            new() { Text = "Sell the ammo but keep the rifle for yourself", NextSceneId = 4, FlagToSet = "KeptRifle" },
            new() { Text = "Tell him you'll sell only if he tells the truth", NextSceneId = 4, FlagToSet = "ThomasToldTruth", ReputationTownChange = 1 }
        };
        db.Scenes.Add(scene3);

        // ===== SCENE 4 =====
        var scene4 = new Scene
        {
            Id = 4,
            Title = "Scene 4: A Stranger at Dusk",
            Description = @"The sun is setting. The sky is a wound — red and gold and purple.

You're about to close the store. Then the door bursts open.

A wounded man stumbles inside. He is pale. His shirt is soaked with blood. He clutches a heavy leather bag against his chest.

""Please... lock the door. I need... I need to leave something here.""

Outside, in the distance, you hear the sound of horses. They are coming.",
            IsEnding = false
        };
        scene4.Choices = new List<Choice>
        {
            new() { Text = "Lock the door and help him", NextSceneId = 5, FlagToSet = "HelpedCole", TrustMayChange = 1 },
            new() { Text = "Tell him to leave. You don't want trouble.", NextSceneId = 5, FlagToSet = "RefusedCole", TrustMayChange = -1 },
            new() { Text = "Draw your weapon and demand his name", NextSceneId = 5, FlagToSet = "ThreatenedCole", TrustMayChange = -1 },
            new() { Text = "Let him stay but keep your distance", NextSceneId = 5, FlagToSet = "CautiousCole" },
            new() { Text = "Step outside and look for the riders", NextSceneId = 5, FlagToSet = "LookedOutside" }
        };
        db.Scenes.Add(scene4);

        // ===== SCENE 5 =====
        var scene5 = new Scene
        {
            Id = 5,
            Title = "Scene 5: The Confession",
            Description = @"The man collapses onto a chair. He drinks water. Then he speaks.

""My name is Cole. I ran from a gang. This bag... it has gold. Twenty kilograms. I stole it from them.""

He pushes the bag toward you.

""You... keep it. If I live, I'll come back. If not... it's yours.""

He stumbles to the back door.

""Don't trust anyone in this town. Not even the Sheriff.""

He disappears into the dark. The bag is still on the floor. It's waiting.",
            IsEnding = false
        };
        scene5.Choices = new List<Choice>
        {
            new() { Text = "Open the bag. See the gold. Decide to keep it.", NextSceneId = 6, FlagToSet = "HasGold", KarmaGreedChange = 1, ReputationGangChange = -1 },
            new() { Text = "Take the bag to the Sheriff immediately", NextSceneId = 6, FlagToSet = "GaveToSheriff", ReputationSheriffChange = 2, ReputationGangChange = 1 },
            new() { Text = "Hide the bag in your storage room", NextSceneId = 6, FlagToSet = "HiddenGold" },
            new() { Text = "Follow Cole outside to see where he goes", NextSceneId = 6, FlagToSet = "FollowedCole" },
            new() { Text = "Leave the bag where it is and go home", NextSceneId = 6, FlagToSet = "IgnoredGold", KarmaGreedChange = -1 }
        };
        db.Scenes.Add(scene5);

        // ===== SCENE 6 =====
        var scene6 = new Scene
        {
            Id = 6,
            Title = "Scene 6: Rumors",
            Description = @"You barely sleep. The bag sits in the corner. You swear you can hear it — heavy, patient, waiting.

The next morning, you walk to the bakery.

Inside, three women stand near the counter. They stop talking when you enter. Then they start again — lower.

""...heard a man was seen last night...""
""...three riders. They didn't come down. They just watched.""

On the horizon, you see them. Three tiny black figures, motionless against the pale sky. Watching.",
            IsEnding = false
        };
        scene6.Choices = new List<Choice>
        {
            new() { Text = "Act normal. Tell no one.", NextSceneId = 7, FlagToSet = "StayedSilent" },
            new() { Text = "Go to the Sheriff and report everything", NextSceneId = 7, FlagToSet = "ToldSheriff", ReputationSheriffChange = 2 },
            new() { Text = "Visit Martha, the saloon owner", NextSceneId = 7, FlagToSet = "VisitedMartha" },
            new() { Text = "Walk to the edge of town to see the riders", NextSceneId = 7, FlagToSet = "SawRiders" },
            new() { Text = "Send a telegram to the U.S. Marshal", NextSceneId = 7, FlagToSet = "CalledMarshal", ReputationSheriffChange = 1 }
        };
        db.Scenes.Add(scene6);

        // ===== SCENE 7 =====
        var scene7 = new Scene
        {
            Id = 7,
            Title = "Scene 7: The Gang Arrives",
            Description = @"Midday. Three riders enter Canyon Creek. Leather coats. Shiny revolvers. Scarred faces.

They tie their horses at the saloon. They drink. Then they walk toward your store.

The leader is a tall man with a scar across his left cheek.

""Hello, shopkeeper. We're looking for a leather bag. Did you see anything?""",
            IsEnding = false
        };
        scene7.Choices = new List<Choice>
        {
            new() { Text = "No, I didn't see anything. (Lie)", NextSceneId = 8, FlagToSet = "LiedToGang", ReputationGangChange = -1 },
            new() { Text = "A man came. But he left nothing. (Half-truth)", NextSceneId = 8, FlagToSet = "HalfTruthToGang" },
            new() { Text = "Why do you want to know? (Resist)", NextSceneId = 8, FlagToSet = "ResistedGang", ReputationGangChange = -2 },
            new() { Text = "Hand over the bag (Surrender)", NextSceneId = 8, FlagToSet = "GaveBagToGang", ReputationGangChange = 2, KarmaGreedChange = -1 },
            new() { Text = "I'll tell you... for a price. (Bargain)", NextSceneId = 8, FlagToSet = "BargainedWithGang", KarmaGreedChange = 1 }
        };
        db.Scenes.Add(scene7);

        // ===== SCENE 8 =====
        var scene8 = new Scene
        {
            Id = 8,
            Title = "Scene 8: The Watcher",
            Description = @"The gang leaves. But one of them stays behind.

He stands across the street, leaning against a post. Watching your store. Not moving. Not hiding.

Hours pass. He's still there.",
            IsEnding = false
        };
        scene8.Choices = new List<Choice>
        {
            new() { Text = "Ignore him. Continue your day.", NextSceneId = 9, FlagToSet = "IgnoredWatcher" },
            new() { Text = "Send a message to the Sheriff", NextSceneId = 9, FlagToSet = "MessagedSheriff", ReputationSheriffChange = 1 },
            new() { Text = "Move the bag to a safer hiding place", NextSceneId = 9, FlagToSet = "MovedGold" },
            new() { Text = "Approach the watcher and talk to him", NextSceneId = 9, FlagToSet = "TalkedToWatcher" },
            new() { Text = "Close the store early and go home", NextSceneId = 9, FlagToSet = "ClosedEarly", ReputationTownChange = -1 }
        };
        db.Scenes.Add(scene8);

        // ===== SCENE 9 =====
        var scene9 = new Scene
        {
            Id = 9,
            Title = "Scene 9: May",
            Description = @"Three days later. A woman enters your store. She is young, tired, with a small child.

""Are you Elias? The shopkeeper?""

You nod.

""I'm May. Cole's wife. I know he came here. I know he's dead.""

She pauses. Her voice cracks.

""But that gold... it's not ours. It belongs to a gang that attacked an innocent family. They killed a father. They burned a farm. I just want... justice.""",
            IsEnding = false
        };
        scene9.Choices = new List<Choice>
        {
            new() { Text = "Give the gold to May so she can return it", NextSceneId = 10, FlagToSet = "GaveGoldToMay", TrustMayChange = 2, KarmaGreedChange = -1, ReputationTownChange = 1 },
            new() { Text = "Keep the gold. Tell her you'll decide yourself.", NextSceneId = 10, FlagToSet = "KeptGoldFromMay", TrustMayChange = -1, KarmaGreedChange = 1 },
            new() { Text = "Tell her to go to the Sheriff", NextSceneId = 10, FlagToSet = "SentMayToSheriff", TrustMayChange = -1 },
            new() { Text = "Suspect she wants the gold for herself", NextSceneId = 10, FlagToSet = "SuspectedMay", TrustMayChange = -2 },
            new() { Text = "Ask her about Cole. Who was he really?", NextSceneId = 10, FlagToSet = "AskedAboutCole", TrustMayChange = 1 }
        };
        db.Scenes.Add(scene9);

        // ===== SCENE 10 =====
        var scene10 = new Scene
        {
            Id = 10,
            Title = "Scene 10: Cole's Story",
            Description = @"May sits down. She holds her child close.

""Cole wasn't a bad man. He was a farmer. We had a small farm near Red Creek. The gang came one night. They burned our barn. They took everything.""

She wipes her eyes.

""He stole their gold. But not for us. He wanted to give it back to the family they destroyed. The Pattersons. They live three days north of here.""",
            IsEnding = false
        };
        scene10.Choices = new List<Choice>
        {
            new() { Text = "Promise to help her finish Cole's mission", NextSceneId = 11, FlagToSet = "PromisedToHelp", TrustMayChange = 2, ReputationTownChange = 1 },
            new() { Text = "Ask how much gold there is. Twenty kilograms is a lot.", NextSceneId = 11, FlagToSet = "AskedAboutGoldAmount", KarmaGreedChange = 1 },
            new() { Text = "Tell her you need time to think", NextSceneId = 11, FlagToSet = "NeededTime" },
            new() { Text = "Offer her and the child a place to stay", NextSceneId = 11, FlagToSet = "OfferedShelter", TrustMayChange = 2, ReputationTownChange = 1 },
            new() { Text = "Ask her to leave. You need to be alone.", NextSceneId = 11, FlagToSet = "SentMayAway", TrustMayChange = -2 }
        };
        db.Scenes.Add(scene10);

        // ===== SCENE 11 =====
        var scene11 = new Scene
        {
            Id = 11,
            Title = "Scene 11: The Reckoning",
            Description = @"That night. The moon is hidden. The street is silent.

Then you hear them. Hooves. Many of them.

The gang leader stands outside your door with five men. Torches in their hands.

""We know it's here, shopkeeper. Hand it over. Or we burn your store down with you inside.""",
            IsEnding = false
        };
        scene11.Choices = new List<Choice>
        {
            new() { Text = "Fight back", NextSceneId = 12, FlagToSet = "ChoseFight" },
            new() { Text = "Surrender and hand over the bag", NextSceneId = 13, FlagToSet = "ChoseSurrender", KarmaGreedChange = -1 },
            new() { Text = "Send for the Sheriff", NextSceneId = 13, FlagToSet = "ChoseSheriff", ReputationSheriffChange = 1 },
            new() { Text = "Make a deal: The gold for our safety", NextSceneId = 13, FlagToSet = "ChoseDeal" },
            new() { Text = "Trick them: hide the gold, tell them it's gone", NextSceneId = 13, FlagToSet = "ChoseTrick" },
            new() { Text = "Run out the back door", NextSceneId = 13, FlagToSet = "ChoseRun", ReputationTownChange = -2 }
        };
        db.Scenes.Add(scene11);

        // ===== SCENE 12 =====
        var scene12 = new Scene
        {
            Id = 12,
            Title = "Scene 12: The Fight",
            Description = @"You grab your rifle. You take position behind the counter.

The first man kicks the door open. You fire.

The night explodes. Bullets tear through wood. Glass shatters.

The leader shouts: ""Burn it! Burn the whole store!""",
            IsEnding = false
        };
        scene12.Choices = new List<Choice>
        {
            new() { Text = "Aim for the leader", NextSceneId = 14, FlagToSet = "AimedLeader" },
            new() { Text = "Aim for the men with torches", NextSceneId = 14, FlagToSet = "AimedTorches" },
            new() { Text = "Fire warning shots and demand they leave", NextSceneId = 14, FlagToSet = "WarningShots" },
            new() { Text = "Throw a lantern to create a fire between you", NextSceneId = 14, FlagToSet = "UsedFire" },
            new() { Text = "Retreat to the back room and barricade", NextSceneId = 14, FlagToSet = "Retreated" }
        };
        db.Scenes.Add(scene12);

        // ===== SCENE 13 =====
        var scene13 = new Scene
        {
            Id = 13,
            Title = "Scene 13: The Deal",
            Description = @"You step outside. Hands raised.

""The gold. I'll give it to you. But you leave this town. You leave May and her child alone.""

The leader smiles.

""You're in no position to negotiate, shopkeeper.""

But he's listening. He wants the gold more than he wants blood.",
            IsEnding = false
        };
        scene13.Choices = new List<Choice>
        {
            new() { Text = "Hand over the gold and trust his word", NextSceneId = 14, FlagToSet = "TrustedGang" },
            new() { Text = "Demand a written promise signed by all of them", NextSceneId = 14, FlagToSet = "DemandedPromise" },
            new() { Text = "Give them half the gold now, half later", NextSceneId = 14, FlagToSet = "GaveHalfGold", KarmaGreedChange = 1 },
            new() { Text = "Secretly keep some gold for May", NextSceneId = 14, FlagToSet = "KeptSomeForMay", TrustMayChange = 2 },
            new() { Text = "Change your mind and fight instead", NextSceneId = 12, FlagToSet = "ChangedMind" }
        };
        db.Scenes.Add(scene13);

        // ===== SCENE 14: ENDING =====
        var scene14 = new Scene
        {
            Id = 14,
            Title = "Ending: The Story Ends",
            Description = @"The dust settles. The night grows quiet.

Your choices have shaped what comes next. Every decision you made — every kindness, every cruelty, every moment of hesitation — has led to this.

The story of Elias of Canyon Creek is complete.",
            IsEnding = true
        };
        db.Scenes.Add(scene14);

        await db.SaveChangesAsync();
    }
}
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
            Title = "صحنه ۱: صبحی معمولی",
            Description = @"کانیون کریک، ۱۸۸۷.

خورشید نارنجی روی خط‌الرأس سنگی شرق می‌چکد. غبار در هوا می‌رقصد، طلایی و آرام. درِ مغازه‌ی عمومی‌ات را باز می‌کنی.

از پنجره، تماشا می‌کنی که شهر بیدار می‌شود. کشاورزی بار روی واگن می‌گذارد. کشیشی ناقوس کلیسا را به صدا درمی‌آورد. بچه‌ها پابرهنه به سمت مکتب می‌دوند.

در با صدای جیرجیر باز می‌شود. پسر بچه‌ای وارد می‌شود.

لباسش پاره است. پاهایش خاکی. حرفی نمی‌زند. به شکلات روی پیشخوان خیره می‌شود.

دست‌هایش می‌لرزد.",
            IsEnding = false
        };
        scene1.Choices = new List<Choice>
        {
            new() { Text = "یک شکلات مجانی به او بده", NextSceneId = 2, FlagToSet = "HelpedBoy", ReputationTownChange = 1, KarmaMercyChange = 1 },
            new() { Text = "بگو دفعه بعد پول بیاورد", NextSceneId = 2, FlagToSet = "RefusedBoy", ReputationTownChange = -1, KarmaGreedChange = 1 },
            new() { Text = "نصف شکلات را به او بده", NextSceneId = 2, FlagToSet = "HalfHelpedBoy", KarmaMercyChange = 1 },
            new() { Text = "نادیده بگیر و به چیدن قفسه‌ها ادامه بده", NextSceneId = 2, FlagToSet = "IgnoredBoy", ReputationTownChange = -1, KarmaMercyChange = -1 },
            new() { Text = "بپرس پدر و مادرش کجا هستند", NextSceneId = 2, FlagToSet = "AskedAboutBoy", SkillPerceptionChange = 1 },
            new() { Text = "👁 [ادراک ۳+] نگاه دقیق‌تری به دست‌های پسر بنداز — چیزی توشون هست", NextSceneId = 2, FlagToSet = "NoticedBoyHands", SkillPerceptionChange = 1, RequiredSkill = "Perception", RequiredSkillLevel = 3 }
        };
        db.Scenes.Add(scene1);

        // ===== SCENE 2 =====
        var scene2 = new Scene
        {
            Id = 2,
            Title = "صحنه ۲: پیرزن",
            Description = @"پسر می‌رود. ناقوس بالای در به صدا درمی‌آید. خانم هارتلی وارد می‌شود.

اگر یک روز هم عمر داشته باشد، هفتاد ساله است. پشتش خمیده، ولی چشم‌هایش مثل شاهین تیز است.

""دو متر از آن پارچه‌ی آبی را می‌خواهم. و جرأت نکن قیمت کامل بگیری، جان.""

روی پیشخوان خم می‌شود. بند انگشتانش سفید شده‌اند.",
            IsEnding = false
        };
        scene2.Choices = new List<Choice>
        {
            new() { Text = "پارچه را به قیمت کامل بده", NextSceneId = 3, FlagToSet = "FullPriceHartley", ReputationTownChange = -1, TrustHartleyChange = -1, KarmaGreedChange = 1 },
            new() { Text = "کمی تخفیف بده", NextSceneId = 3, FlagToSet = "DiscountedHartley", ReputationTownChange = 1, TrustHartleyChange = 1, KarmaHonorChange = 1 },
            new() { Text = "قیمت را پایین نیاور و محکم بمان", NextSceneId = 3, FlagToSet = "StubbornHartley", ReputationTownChange = -1, TrustHartleyChange = -1, SkillIntimidationChange = 1 },
            new() { Text = "از او درباره‌ی شایعات شهر بپرس", NextSceneId = 3, FlagToSet = "AskedRumors", SkillPerceptionChange = 1, TrustHartleyChange = 1 },
            new() { Text = "از پسر بچه برایش بگو و بپرس می‌شناسدش", NextSceneId = 3, FlagToSet = "MentionedBoy", TrustHartleyChange = 1, KarmaMercyChange = 1 }
        };
        db.Scenes.Add(scene2);

        // ===== SCENE 3 =====
        var scene3 = new Scene
        {
            Id = 3,
            Title = "صحنه ۳: کشاورز",
            Description = @"خانم هارتلی می‌رود، زیر لب چیزی می‌گوید.

در دوباره باز می‌شود. کشاورزی به نام تام وارد می‌شود. سال‌هاست می‌شناسی‌اش. توی لبه‌ی شرقی شهر ذرت می‌کارد.

امروز، دست‌هایش می‌لرزند.

""تمام فشنگ‌هایی که داری رو بده. و... یه تفنگ برای فروش داری؟ یه تفنگ خوب.""

صدایش زیادی آرام است. زیادی کنترل‌شده.",
            IsEnding = false
        };
        scene3.Choices = new List<Choice>
        {
            new() { Text = "هر چیزی که خواست به او بفروش", NextSceneId = 4, FlagToSet = "SoldAmmoToThomas", ReputationTownChange = 1, TrustTomChange = 2 },
            new() { Text = "بپرس چه مشکلی دارد", NextSceneId = 4, FlagToSet = "AskedThomas", TrustTomChange = 2, SkillEmpathyChange = 1 },
            new() { Text = "از فروش امتناع کن و بگو آرام شود", NextSceneId = 4, FlagToSet = "RefusedThomas", ReputationTownChange = -1, TrustTomChange = -1, KarmaMercyChange = 1 },
            new() { Text = "فشنگ‌ها را بفروش ولی تفنگ را برای خودت نگه دار", NextSceneId = 4, FlagToSet = "KeptRifle", TrustTomChange = 1, KarmaGreedChange = 1 },
            new() { Text = "بگو فقط اگر حقیقت را بگوید می‌فروشی", NextSceneId = 4, FlagToSet = "ThomasToldTruth", TrustTomChange = 3, SkillPersuasionChange = 1 },
            new() { Text = "🗣 [متقاعدسازی ۴+] آرامش کن و بگو چی توی ذهنشه رو بگه", NextSceneId = 4, FlagToSet = "ThomasOpenedUp", TrustTomChange = 3, SkillEmpathyChange = 1, RequiredSkill = "Persuasion", RequiredSkillLevel = 4 }
        };
        db.Scenes.Add(scene3);

        // ===== SCENE 4 =====
        var scene4 = new Scene
        {
            Id = 4,
            Title = "صحنه ۴: غریبه‌ای در غروب",
            Description = @"خورشید در حال غروب است. آسمان یک زخم است — سرخ و طلایی و بنفش.

می‌خواهی مغازه را ببندی. دستت روی دستگیره است.

ناگهان در با ضربه باز می‌شود.

مردی زخمی داخل می‌افتد. رنگ‌پریده. پیراهنش خیس خون است، تیره و تازه. نفس‌نفس می‌زند.

یک کیسه‌ی چرمی سنگین را به سینه‌اش فشار می‌دهد.

""لطفاً... در رو قفل کن. باید... باید یه چیزی اینجا بذارم.""

بیرون، صدای سم اسب‌ها را می‌شنوی. آن‌ها می‌آیند.",
            IsEnding = false
        };
        scene4.Choices = new List<Choice>
        {
            new() { Text = "در را قفل کن و کمکش کن", NextSceneId = 5, FlagToSet = "HelpedCole", TrustColeChange = 2, KarmaMercyChange = 1 },
            new() { Text = "بگو برود. دردسر نمی‌خواهی.", NextSceneId = 5, FlagToSet = "RefusedCole", TrustColeChange = -3, KarmaMercyChange = -1 },
            new() { Text = "سلاحت را بکش و اسمش را بپرس", NextSceneId = 5, FlagToSet = "ThreatenedCole", TrustColeChange = -2, SkillIntimidationChange = 1, KarmaHonorChange = -1 },
            new() { Text = "بگذار بماند ولی فاصله‌ات را حفظ کن", NextSceneId = 5, FlagToSet = "CautiousCole", TrustColeChange = -1, SkillPerceptionChange = 1 },
            new() { Text = "برو بیرون و سوارکارها را ببین", NextSceneId = 5, FlagToSet = "LookedOutside", SkillPerceptionChange = 2 },
            new() { Text = "👁 [ادراک ۴+] از پنجره‌ی پشتی ببین چند نفر دارن میان", NextSceneId = 5, FlagToSet = "CountedRiders", SkillPerceptionChange = 2, RequiredSkill = "Perception", RequiredSkillLevel = 4 }
        };
        db.Scenes.Add(scene4);

        // ===== SCENE 5 =====
        var scene5 = new Scene
        {
            Id = 5,
            Title = "صحنه ۵: اعتراف",
            Description = @"در را قفل می‌کنی. قفل با صدای سنگینی جا می‌افتد.

مرد روی صندلی می‌افتد. کیسه را رها می‌کند. با صدای سنگینی روی زمین می‌افتد — سنگین‌تر از آنکه خالی باشد.

""اسم من ری‌یه. از یه دار و دسته فرار کردم. این کیسه... توش طلاست. بیست کیلو.""

به تو نگاه می‌کند. چشم‌هایش به رنگ سنگ خیس است.

""ازشون دزدیدم. دارن میان دنبالم. نمی‌تونم حملش کنم. نمی‌تونم دیگه فرار کنم.""

کیسه را با پایش به سمت تو هل می‌دهد.

""تو... نگهش دار. اگه زنده موندم، برمی‌گردم. اگه نه... مال خودته.""

بلند می‌شود. به سمت درِ پشتی می‌رود.

""به هیچ‌کس توی این شهر اعتماد نکن. حتی به کلانتر.""

در تاریکی ناپدید می‌شود. کیسه هنوز روی زمین است.",
            IsEnding = false
        };
        scene5.Choices = new List<Choice>
        {
            new() { Text = "کیسه را باز کن. طلا را ببین. تصمیم بگیر نگهش داری.", NextSceneId = 6, FlagToSet = "HasGold", KarmaGreedChange = 2, ReputationGangChange = -1 },
            new() { Text = "همین حالا کیسه را به کلانتر ببر", NextSceneId = 6, FlagToSet = "GaveToSheriff", ReputationSheriffChange = 2, TrustSheriffChange = 2, KarmaHonorChange = 2, ReputationGangChange = 1 },
            new() { Text = "کیسه را در انبار پنهان کن", NextSceneId = 6, FlagToSet = "HiddenGold", KarmaGreedChange = 1, SkillPerceptionChange = 1 },
            new() { Text = "ری را دنبال کن و ببین کجا می‌رود", NextSceneId = 6, FlagToSet = "FollowedCole", TrustColeChange = 1, SkillPerceptionChange = 1 },
            new() { Text = "کیسه را همان‌جا بگذار و به خانه برو", NextSceneId = 6, FlagToSet = "IgnoredGold", KarmaMercyChange = 1, KarmaGreedChange = -1 },
            new() { Text = "👁 [ادراک ۵+] چیزی روی دست ری دیدی — یه خالکوبی عجیب", NextSceneId = 6, FlagToSet = "SawColeTattoo", SkillPerceptionChange = 2, RequiredSkill = "Perception", RequiredSkillLevel = 5 }
        };
        db.Scenes.Add(scene5);

        // ===== SCENE 6 =====
        var scene6 = new Scene
        {
            Id = 6,
            Title = "صحنه ۶: شایعات",
            Description = @"به سختی می‌خوابی.

کیسه در گوشه‌ی اتاق خوابت نشسته است. قسم می‌خوری که می‌شنوی‌اش — سنگین، صبور، منتظر.

صبح روز بعد، به نانوایی می‌روی. سه زن نزدیک پیشخوان ایستاده‌اند. وقتی وارد می‌شوی، ساکت می‌شوند.

""...شنیدم دیشب یه مرد دیده شده...""
""...زخمی. در حال فرار...""
""...و امروز صبح، سه سوارکار روی خط‌الرأس. فقط تماشا کردن.""

روی افق، آن‌ها را می‌بینی. سه پیکر کوچک سیاه. بی‌حرکت.

تماشا می‌کنند.",
            IsEnding = false
        };
        scene6.Choices = new List<Choice>
        {
            new() { Text = "عادی رفتار کن. به کسی چیزی نگو.", NextSceneId = 7, FlagToSet = "StayedSilent", SkillPerceptionChange = 1 },
            new() { Text = "برو پیش کلانتر و همه چیز را بگو", NextSceneId = 7, FlagToSet = "ToldSheriff", ReputationSheriffChange = 2, TrustSheriffChange = 2 },
            new() { Text = "به دیدن مارتا، صاحب سالن، برو", NextSceneId = 7, FlagToSet = "VisitedMartha", TrustMarthaChange = 2 },
            new() { Text = "تا لبه‌ی شهر برو و سوارکارها را ببین", NextSceneId = 7, FlagToSet = "SawRiders", SkillPerceptionChange = 2 },
            new() { Text = "تلگرافی برای مارشال ایالات متحده بفرست", NextSceneId = 7, FlagToSet = "CalledMarshal", ReputationSheriffChange = 1, TrustSheriffChange = 1 }
        };
        db.Scenes.Add(scene6);

        // ===== SCENE 7 =====
        var scene7 = new Scene
        {
            Id = 7,
            Title = "صحنه ۷: ورود دار و دسته",
            Description = @"ظهر.

سه سوارکار وارد کانیون کریک می‌شوند. کاپشن‌های چرمی. هفت‌تیرهای براق. صورت‌های زخم‌خورده.

رئیس، مردی بلندقد با زخمی روی گونه‌ی چپش است. اسمش جاناتانه.

""سلام مغازه‌دار. دنبال یه کیسه‌ی چرمی هستیم. یه مرد دیشب از اینجا رد شده. چیزی دیدی؟""",
            IsEnding = false
        };
        scene7.Choices = new List<Choice>
        {
            new() { Text = "نه، چیزی ندیدم. (دروغ)", NextSceneId = 8, FlagToSet = "LiedToGang", ReputationGangChange = -2, SkillPersuasionChange = 1 },
            new() { Text = "یه مرد اومد. ولی چیزی نذاشت. (نیمه‌راست)", NextSceneId = 8, FlagToSet = "HalfTruthToGang", ReputationGangChange = -1, SkillPerceptionChange = 1 },
            new() { Text = "چرا می‌خوای بدونی؟ (مقاومت)", NextSceneId = 8, FlagToSet = "ResistedGang", ReputationGangChange = -3, SkillIntimidationChange = 2 },
            new() { Text = "کیسه را تحویل بده (تسلیم)", NextSceneId = 8, FlagToSet = "GaveBagToGang", ReputationGangChange = 3, KarmaHonorChange = -2 },
            new() { Text = "بهت می‌گم... به یه قیمت. (معامله)", NextSceneId = 8, FlagToSet = "BargainedWithGang", ReputationGangChange = 1, KarmaGreedChange = 2, SkillPersuasionChange = 2 },
            new() { Text = "🗣 [متقاعدسازی ۶+] با یه داستان، حواسشون رو پرت کن", NextSceneId = 8, FlagToSet = "TrickedGang", ReputationGangChange = -1, SkillPersuasionChange = 2, RequiredSkill = "Persuasion", RequiredSkillLevel = 6 }
        };
        db.Scenes.Add(scene7);

        // ===== SCENE 8 =====
        var scene8 = new Scene
        {
            Id = 8,
            Title = "صحنه ۸: نگهبان",
            Description = @"دار و دسته می‌روند. ولی یکی از آن‌ها می‌ماند.

او در آن طرف خیابان ایستاده، به تیرکی تکیه داده. مغازه‌ی تو را تماشا می‌کند.

ساعت‌ها می‌گذرد. هنوز آنجاست.",
            IsEnding = false
        };
        scene8.Choices = new List<Choice>
        {
            new() { Text = "نادیده بگیر. به روزت ادامه بده.", NextSceneId = 9, FlagToSet = "IgnoredWatcher" },
            new() { Text = "پیامی برای کلانتر بفرست", NextSceneId = 9, FlagToSet = "MessagedSheriff", ReputationSheriffChange = 1, TrustSheriffChange = 1 },
            new() { Text = "کیسه را به جای امن‌تری منتقل کن", NextSceneId = 9, FlagToSet = "MovedGold", SkillPerceptionChange = 1 },
            new() { Text = "به نگهبان نزدیک شو و با او حرف بزن", NextSceneId = 9, FlagToSet = "TalkedToWatcher", SkillPersuasionChange = 1 },
            new() { Text = "زودتر مغازه را ببند و به خانه برو", NextSceneId = 9, FlagToSet = "ClosedEarly", ReputationTownChange = -1 }
        };
        db.Scenes.Add(scene8);

        // ===== SCENE 9 =====
        var scene9 = new Scene
        {
            Id = 9,
            Title = "صحنه ۹: سارا",
            Description = @"سه روز بعد. زنی وارد مغازه‌ات می‌شود. جوان است، خسته، با بچه‌ی کوچکی که دستش را گرفته.

""شما جان هستید؟ مغازه‌دار؟""

سر تکان می‌دهی.

""من سارا هستم. زن ری. می‌دونم اومد اینجا. می‌دونم مرده.""

مکث می‌کند. صدایش می‌شکند.

""ولی اون طلا... مال ما نیست. مال دار و دسته‌ایه که به یه خانواده‌ی بی‌گناه حمله کردن. من فقط... عدالت می‌خوام.""",
            IsEnding = false
        };
        scene9.Choices = new List<Choice>
        {
            new() { Text = "طلا را به سارا بده تا برگرداند", NextSceneId = 10, FlagToSet = "GaveGoldToMay", TrustMayChange = 3, KarmaMercyChange = 2, ReputationTownChange = 1 },
            new() { Text = "طلا را نگه دار. بگو خودت تصمیم می‌گیری.", NextSceneId = 10, FlagToSet = "KeptGoldFromMay", TrustMayChange = -2, KarmaGreedChange = 2 },
            new() { Text = "بگو برود پیش کلانتر", NextSceneId = 10, FlagToSet = "SentMayToSheriff", TrustMayChange = -1 },
            new() { Text = "شک کن که خودش طلا را می‌خواهد", NextSceneId = 10, FlagToSet = "SuspectedMay", TrustMayChange = -3 },
            new() { Text = "از ری بپرس. او واقعاً کی بود؟", NextSceneId = 10, FlagToSet = "AskedAboutCole", TrustMayChange = 2, SkillEmpathyChange = 1 },
            new() { Text = "💗 [همدلی ۵+] بذار بچه‌اش بشینه، به سارا آرامش بده", NextSceneId = 10, FlagToSet = "ComfortedSara", TrustMayChange = 3, SkillEmpathyChange = 2, RequiredSkill = "Empathy", RequiredSkillLevel = 5 }
        };
        db.Scenes.Add(scene9);

        // ===== SCENE 10 =====
        var scene10 = new Scene
        {
            Id = 10,
            Title = "صحنه ۱۰: داستان ری",
            Description = @"سارا می‌نشیند. بچه‌اش را نزدیک خود نگه می‌دارد.

""ری مرد بدی نبود. یه کشاورز بود. یه مزرعه‌ی کوچیک نزدیک رد کریک داشتیم. دار و دسته یه شب اومدن. انبارمون رو سوزوندن. همه چیز رو بردن.""

چشم‌هایش را پاک می‌کند.

""طلاشون رو دزدید. ولی نه برای ما. می‌خواست به خانواده‌ای برگردونه که نابودش کردن. پترسون‌ها. سه روز بالاتر از اینجا زندگی می‌کنن.""",
            IsEnding = false
        };
        scene10.Choices = new List<Choice>
        {
            new() { Text = "قول بده مأموریت ری را تمام کنی", NextSceneId = 11, FlagToSet = "PromisedToHelp", TrustMayChange = 3, ReputationTownChange = 1 },
            new() { Text = "بپرس چقدر طلا هست. بیست کیلو خیلیه.", NextSceneId = 11, FlagToSet = "AskedAboutGoldAmount", KarmaGreedChange = 2 },
            new() { Text = "بگو وقت می‌خواهی فکر کنی", NextSceneId = 11, FlagToSet = "NeededTime" },
            new() { Text = "به او و بچه‌اش جا بده", NextSceneId = 11, FlagToSet = "OfferedShelter", TrustMayChange = 3, KarmaMercyChange = 2 },
            new() { Text = "بگو برود. باید تنها باشی.", NextSceneId = 11, FlagToSet = "SentMayAway", TrustMayChange = -2, KarmaMercyChange = -1 }
        };
        db.Scenes.Add(scene10);

        // ===== SCENE 11 =====
        var scene11 = new Scene
        {
            Id = 11,
            Title = "صحنه ۱۱: تسویه حساب",
            Description = @"آن شب. ماه پنهان است. خیابان ساکت.

بعد می‌شنوی‌شان. صدای سم. تعداد زیادی.

جاناتان با پنج مرد بیرون درِ تو ایستاده. مشعل در دست دارند.

""می‌دونیم اینجاست، مغازه‌دار. تحویلش بده. وگرنه مغازه‌ت رو با خودت آتیش می‌زنیم.""",
            IsEnding = false
        };
        scene11.Choices = new List<Choice>
        {
            new() { Text = "مقاومت کن", NextSceneId = 12, FlagToSet = "ChoseFight", SkillCombatChange = 1 },
            new() { Text = "تسلیم شو و کیسه را تحویل بده", NextSceneId = 13, FlagToSet = "ChoseSurrender", KarmaHonorChange = -2, KarmaMercyChange = -1 },
            new() { Text = "به دنبال کلانتر بفرست", NextSceneId = 13, FlagToSet = "ChoseSheriff", ReputationSheriffChange = 1, TrustSheriffChange = 1 },
            new() { Text = "معامله کن: طلا برای امنیت ما", NextSceneId = 13, FlagToSet = "ChoseDeal", SkillPersuasionChange = 1 },
            new() { Text = "گولشان بزن: طلا را پنهان کن، بگو رفته", NextSceneId = 13, FlagToSet = "ChoseTrick", SkillPersuasionChange = 1 },
            new() { Text = "از درِ پشتی فرار کن", NextSceneId = 13, FlagToSet = "ChoseRun", ReputationTownChange = -2, KarmaHonorChange = -1 },
            new() { Text = "🔫 [نبرد ۶+] تفنگت رو بکش و اول شلیک کن", NextSceneId = 12, FlagToSet = "ChoseFight", SkillCombatChange = 2, RequiredSkill = "Combat", RequiredSkillLevel = 6 }
        };
        db.Scenes.Add(scene11);

        // ===== SCENE 12 =====
        var scene12 = new Scene
        {
            Id = 12,
            Title = "صحنه ۱۲: نبرد",
            Description = @"تفنگت را برمی‌داری. پشت پیشخوان موضع می‌گیری.

اولین مرد در را با لگد باز می‌کند. شلیک می‌کنی.

شب منفجر می‌شود. گلوله‌ها چوب را می‌درند. شیشه خرد می‌شود.

جاناتان فریاد می‌زند: ""آتیشش بزن! کل مغازه رو بسوزون!""",
            IsEnding = false
        };
        scene12.Choices = new List<Choice>
        {
            new() { Text = "به سمت جاناتان نشانه بگیر", NextSceneId = 14, FlagToSet = "AimedLeader", SkillCombatChange = 1 },
            new() { Text = "به سمت مردهای مشعل‌دار نشانه بگیر", NextSceneId = 14, FlagToSet = "AimedTorches", SkillCombatChange = 1 },
            new() { Text = "تیر هشدار بزن و بخواه بروند", NextSceneId = 14, FlagToSet = "WarningShots", KarmaMercyChange = 1 },
            new() { Text = "فانوسی را پرت کن تا آتش بسازد", NextSceneId = 14, FlagToSet = "UsedFire", SkillPerceptionChange = 1 },
            new() { Text = "به اتاق پشتی عقب‌نشینی کن", NextSceneId = 14, FlagToSet = "Retreated", SkillPerceptionChange = 1 }
        };
        db.Scenes.Add(scene12);

        // ===== SCENE 13 =====
        var scene13 = new Scene
        {
            Id = 13,
            Title = "صحنه ۱۳: معامله",
            Description = @"بیرون می‌آیی. دست‌ها بالا.

""طلا. تحویلت می‌دم. ولی این شهر رو ترک کن. سارا و بچه‌اش رو تنها بذار. هرگز برنگرد.""

جاناتان لبخند می‌زند.

""تو موقعیت معامله نیستی، مغازه‌دار.""

ولی گوش می‌دهد. طلا را بیشتر از خون می‌خواهد.",
            IsEnding = false
        };
        scene13.Choices = new List<Choice>
        {
            new() { Text = "طلا را تحویل بده و به حرفش اعتماد کن", NextSceneId = 14, FlagToSet = "TrustedGang" },
            new() { Text = "قسم‌نامه‌ی کتبی با امضای همه بخواه", NextSceneId = 14, FlagToSet = "DemandedPromise", SkillPersuasionChange = 1 },
            new() { Text = "نصف طلا را حالا بده، نصف بعداً", NextSceneId = 14, FlagToSet = "GaveHalfGold", KarmaGreedChange = 1 },
            new() { Text = "مخفیانه مقداری طلا برای سارا نگه دار", NextSceneId = 14, FlagToSet = "KeptSomeForMay", TrustMayChange = 2, KarmaMercyChange = 1 },
            new() { Text = "نظرت را عوض کن و بجنگ", NextSceneId = 12, FlagToSet = "ChangedMind", SkillCombatChange = 1 }
        };
        db.Scenes.Add(scene13);

        // ===== SCENE 14: ENDING =====
        var scene14 = new Scene
        {
            Id = 14,
            Title = "پایان",
            Description = "پایان.",
            IsEnding = true
        };
        db.Scenes.Add(scene14);

        await db.SaveChangesAsync();
    }
}
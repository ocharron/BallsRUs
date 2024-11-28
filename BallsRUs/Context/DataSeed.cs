using BallsRUs.Entities;
using BallsRUs.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BallsRUs.Context
{
    public static class DataSeed
    {
        public static readonly PasswordHasher<User> _passwordHasher = new();

        public static void Seed(this ModelBuilder builder)
        {
            // Ajouter les rôles de base
            IdentityRole<Guid> adminRole = AddRole(builder, Constants.ROLE_ADMIN);
            _ = AddRole(builder, Constants.ROLE_UTILISATEUR);

            // Ajouter l'utilisateur admin
            User adminUser = AddUser(builder, "admin@ballsrus.ca", "Querty123!", "Mister", "Admin", "1234567890");
            AddUserToRole(builder, adminUser, adminRole);

            // Ajouter l'admin à une addresse
            Address address = AddAddressToUser(builder, adminUser, "123 rue du Hamburger", "Saint-Amable", "Québec", "Canada", "H3D 8H4");

            // Ajouter les catégories de produits.
            #region Catégories
            Category batons = AddCategory(builder, "Bâtons", "Découvrez notre sélection de bâtons de golf de qualité supérieure, conçus pour améliorer votre jeu sur le parcours. Trouvez l'équipement parfait pour affiner votre swing et optimiser votre précision. Faites le choix de l'excellence sur le green.", "~/img/categories/batons.jpg", DateTime.Now.AddDays(-365));
            Category boisDeDepart = AddCategory(builder, "Bois de départ", "Découvrez nos bois de départ de haute performance pour des coups longs et précis sur le parcours. Excellents pour maximiser la distance et la précision. Rendez chaque départ une opportunité de réussite.", "~/img/categories/boisDeDepart.jpg", DateTime.Now.AddDays(-365), batons.Id);
            Category fers = AddCategory(builder, "Fers", "Découvrez nos fers haut de gamme, spécialement conçus pour des coups précis et maîtrisés sur le parcours. Grâce à nos fers, perfectionnez votre précision et atteignez vos objectifs de jeu. Transformez chaque coup sur le fairway en une expérience de réussite inégalée.", "~/img/categories/fers.jpg", DateTime.Now.AddDays(-365), batons.Id);
            Category boisAllee = AddCategory(builder, "Bois d'allée", "Explorez notre gamme de bois d'allée de haute qualité, conçus pour des coups précis et puissants sur le fairway. Transformez chaque départ en une opportunité de succès. Optez pour l'excellence sur le green avec nos bois d'allée de première classe.", "~/img/categories/boisAllee.jpg", DateTime.Now.AddDays(-365), batons.Id);
            Category hybrides = AddCategory(builder, "Hybrides", "Découvrez notre gamme d'hybrides de premier choix, conçus pour des coups précis et puissants tout le long du fairway. Donnez à chaque coup la chance de briller. Optez pour l'excellence avec nos hybrides haut de gamme.", "~/img/categories/hybrides.jpg", DateTime.Now.AddDays(-365), batons.Id);
            Category cocheurs = AddCategory(builder, "Cocheurs", "Découvrez nos cocheurs de qualité supérieure, minutieusement conçus pour des approches précises et contrôlées sur le green. Affinez votre jeu à chaque coup. Choisissez la perfection avec nos cocheurs haut de gamme.", "~/img/categories/cocheurs.jpg", DateTime.Now.AddDays(-365), batons.Id);
            Category fersDroits = AddCategory(builder, "Fers droits", "Explorez notre sélection de fers droits haut de gamme, méticuleusement conçus pour des coups précis et droits sur le green. Perfectionnez votre jeu à chaque tir. Choisissez l'excellence avec nos fers droits de première classe.", "~/img/categories/fersDroits.jpg", DateTime.Now.AddDays(-365), batons.Id);
            Category balles = AddCategory(builder, "Balles", "Découvrez notre vaste assortiment de balles de golf, conçues pour répondre aux besoins de joueurs de tous niveaux. Nos balles de golf allient technologie avancée et performances exceptionnelles, offrant la distance et la précision nécessaires pour briller sur le parcours.", "~/img/categories/balles.jpg", DateTime.Now.AddDays(-365));
            Category vetements = AddCategory(builder, "Vêtements", "Découvrez notre sélection de vêtements de golf de qualité supérieure, conçus pour allier style et performance sur le parcours. Nos vêtements intègrent des matériaux de haute technologie pour vous offrir un confort optimal, quelles que soient les conditions.", "~/img/categories/vetements.jpg", DateTime.Now.AddDays(-365));
            Category polos = AddCategory(builder, "Polos", "Explorez notre collection de polos de golf, une fusion parfaite de style et de performance. Nos polos sont conçus pour vous offrir une élégance inégalée sur le parcours tout en vous assurant un confort optimal, même lors des journées les plus ensoleillées.", "~/img/categories/polos.jpg", DateTime.Now.AddDays(-365), vetements.Id);
            Category pantalons = AddCategory(builder, "Pantalons", "Explorez notre gamme de pantalons de golf, une combinaison parfaite de style et de performance. Nos pantalons sont conçus pour allier élégance sur le parcours et un confort optimal, quelles que soient les conditions.", "~/img/categories/pantalons.jpg", DateTime.Now.AddDays(-365), vetements.Id);
            Category chaussures = AddCategory(builder, "Chaussures", "Découvrez notre collection de chaussures de golf, alliant style et performance avec perfection. Nos chaussures sont conçus pour vous offrir une élégance exceptionnelle sur le parcours tout en assurant un confort inégalé, quelles que soient les conditions du terrain.", "~/img/categories/souliers.jpg", DateTime.Now.AddDays(-365), vetements.Id);
            Category sacs = AddCategory(builder, "Sacs", "Explorez notre gamme complète de sacs de golf, conçus pour répondre à tous vos besoins sur le parcours. Nos sacs combinent style et fonctionnalité, offrant une solution de transport élégante pour vos clubs et équipements.", "~/img/categories/sacs.jpg", DateTime.Now.AddDays(-365));
            Category sacsSurPied = AddCategory(builder, "Sacs sur pied", "Découvrez notre sélection de sacs sur pied de golf, l'équilibre parfait entre style et fonctionnalité. Nos sacs sur pied sont conçus pour vous offrir une solution de transport élégante pour vos clubs, combinée à la commodité de se tenir debout sur le parcours.", "~/img/categories/sacsSurPied.jpg", DateTime.Now.AddDays(-365), sacs.Id);
            Category sacsPourChariot = AddCategory(builder, "Sacs pour chariot", "Découvrez notre sélection de sacs pour chariot de golf, alliant élégance et utilité de manière harmonieuse. Nos sacs pour chariot sont spécialement conçus pour transporter vos clubs avec style tout en s'intégrant parfaitement à votre chariot.", "~/img/categories/sacsPourChariot.jpg", DateTime.Now.AddDays(-365), sacs.Id);
            Category accessoires = AddCategory(builder, "Accessoires", "Explorez notre vaste collection d'accessoires de golf, conçus pour améliorer chaque aspect de votre jeu. Nos accessoires offrent des solutions innovantes pour vous aider à préparer vos coups, à prendre soin de vos clubs et à rester à l'aise sur le parcours.", "~/img/categories/accessoires.jpg", DateTime.Now.AddDays(-365));
            Category telemetres = AddCategory(builder, "Télémètres", "Découvrez notre sélection de télémètres de golf, les outils parfaits pour améliorer votre précision sur le parcours. Nos télémètres sont conçus pour mesurer avec une grande précision les distances, vous permettant ainsi de choisir le bon club à chaque coup.", "~/img/categories/telemetres.jpg", DateTime.Now.AddDays(-365), accessoires.Id);
            Category gps = AddCategory(builder, "GPS", "Explorez notre gamme de GPS de golf, l'outil ultime pour améliorer votre jeu sur le parcours. Nos GPS de golf sont conçus pour vous offrir des informations précises sur les distances, les obstacles et les caractéristiques du terrain, vous aidant ainsi à prendre des décisions éclairées à chaque coup.", "~/img/categories/gps.jpg", DateTime.Now.AddDays(-365), accessoires.Id);
            #endregion

            // Ajouter les produits
            #region Produits
            Product productBBD8943TMD = AddProduct(builder, "BBD8943TMD", "Taylormade Stealth2 - Bois de départ", "Taylormade", "Stealth2", "~/img/products/BBD8943TMD.jpg", "Le Taylormade Stealth2: la puissance redéfinie pour des coups longs et précis.", "Découvrez le driver Taylormade Stealth2, une véritable révolution dans le monde du golf. Ce club redéfinit la puissance et la précision, offrant des performances exceptionnelles sur le parcours. Grâce à sa construction innovante en acier au carbone, il permet des vitesses de balle impressionnantes tout en assurant une stabilité inégalée. Son design aérodynamique amélioré réduit la traînée, favorisant des coups plus longs et plus droits. Le Taylormade Stealth2 est le choix incontournable pour les golfeurs désireux d'améliorer leur jeu et d'atteindre de nouveaux sommets de performance.", 310, null, 25, 799.99m, DateTime.Today.AddDays(-294));
            Product productBBD3845TMD = AddProduct(builder, "BBD3845TMD", "Taylormade SIM2 Max Draw - Bois de départ", "Taylormade", "SIM2 Max Draw", "~/img/products/BBD3845TMD.jpg", "Le Taulormade SIM2 Max Draw: Conçu pour des trajectoires droites.", "Découvrez le Taylormade SIM2 Max Draw, un driver conçu pour offrir des trajectoires droites et une tolérance exceptionnelle. Ce club intègre des technologies avancées pour aider les golfeurs à éviter les slices et à frapper plus droit. Grâce à son poids déplacé vers l'arrière et une face plus ouverte, le SIM2 Max Draw favorise un angle de lancement plus élevé et une trajectoire plus droite. Le design aérodynamique amélioré réduit la traînée pour des coups plus longs et plus précis. Le Taylormade SIM2 Max Draw est le choix idéal pour les golfeurs cherchant à améliorer leur jeu et à maintenir la balle sur la trajectoire souhaitée.", 315, null, 16, 499.97m, DateTime.Today.AddDays(-256));
            Product productBBD4058CAL = AddProduct(builder, "BBD4058CAL", "Callaway Rogue ST Max - Bois de départ", "Callaway", "Rogue ST Max", "~/img/products/BBD4058CAL.jpg", "Le Callaway Rogue ST Max: Puissance inégalée.", "Découvrez le driver Callaway Rogue ST Max, un chef-d'œuvre de puissance et de performance. Conçu avec des technologies de pointe, ce driver offre une combinaison parfaite de distance et de contrôle, permettant aux golfeurs de repousser leurs limites. La construction innovante et l'aérodynamisme avancé optimisent la vitesse de la balle pour des coups longs et précis. Le poids stratégiquement placé permet de maximiser le MOI, offrant une tolérance exceptionnelle. Le Callaway Rogue ST Max est l'arme ultime pour les golfeurs en quête de performances exceptionnelles sur le parcours, garantissant des coups explosifs et des trajectoires impressionnantes.", 310, null, 18, 549.97m, DateTime.Today.AddDays(-274));
            Product productBFE1849CAL = AddProduct(builder, "BFE1849CAL", "Callaway Paradym X - Ensemble de fers", "Callaway", "Paradym X", "~/img/products/BFE1849CAL.jpg", "Les Callaway Paradym X: Performances de pointe.", "Découvrez l'ensemble de fers Callaway Paradym X, une fusion parfaite de technologie de pointe et de précision. Ces fers offrent des performances exceptionnelles sur le parcours, grâce à leur conception innovante. Leur construction en acier de haute qualité garantit une distance impressionnante, tandis que le centre de gravité bas améliore la tolérance et favorise un angle de lancement optimal. Les fers Paradym X sont conçus pour répondre aux besoins des golfeurs de tous niveaux, offrant un équilibre parfait entre puissance et contrôle. Optez pour la qualité et la performance avec l'ensemble de fers Paradym X, la solution idéale pour améliorer votre jeu sur le green.", null, null, 8, 1899.99m, DateTime.Today.AddDays(-197));
            Product productBFE5421CAL = AddProduct(builder, "BFE5421CAL", "Callaway Big Bertha '23 - Ensemble de fers", "Callaway", "Big Bertha 23", "~/img/products/BFE5421CAL.jpg", "Les Callaway Big Bertha 23: Puissance et précision.", "Découvrez l'ensemble de fers Callaway Big Bertha 23, une référence en matière de performances et de polyvalence. Ces fers offrent une combinaison exceptionnelle de distance et de contrôle, grâce à leur conception de pointe. La technologie avancée permet d'optimiser la vitesse de balle, tandis que le centre de gravité précis favorise des trajectoires précises et un angle de lancement optimal. Les fers Big Bertha 23 sont conçus pour répondre aux besoins des golfeurs de tous niveaux, offrant une qualité inégalée. Optez pour l'ensemble de fers Big Bertha 23 pour des performances exceptionnelles sur le parcours, des coups longs et précis qui feront la différence à chaque partie.", null, null, 12, 1399.99m, DateTime.Today.AddDays(-209));
            Product productBFE4924CLE = AddProduct(builder, "BFE4924CLE", "Cleveland Launcher UHX - Ensemble de fers", "Cleveland", "Launcher UHX", "~/img/products/BFE4924CLE.jpg", "Les Cleveland Launcher UHX: Puissance et polyvalence.", "Découvrez l'ensemble de fers Cleveland Launcher UHX, une combinaison parfaite de puissance et de polyvalence pour votre jeu de golf. Ces fers intègrent des technologies de pointe pour offrir des performances exceptionnelles sur le parcours. Leur conception innovante permet d'optimiser la vitesse de balle, tandis que le poids stratégiquement placé favorise un centre de gravité bas pour des trajectoires précises et un contrôle optimal. Les fers Launcher UHX sont conçus pour répondre aux besoins des golfeurs de tous niveaux, offrant une qualité inégalée. Optez pour cet ensemble pour des performances de pointe, des coups longs et précis qui feront la différence à chaque partie.", null, null, 6, 799.99m, DateTime.Today.AddDays(-310));
            Product productBBA8492COB = AddProduct(builder, "BBA8492COB", "Cobra Speedzone King SZ - Bois d'allée", "Cobra", "Speedzone King SZ", "~/img/products/BBA8492COB.jpg", "Le Cobra Speedzone King SZ: Vitesse et précision.", "Découvrez le bois d'allée Cobra Speedzone King SZ, un club qui allie vitesse et précision pour des coups spectaculaires sur le parcours. Ce bois d'allée intègre des technologies de pointe pour maximiser la vitesse de la balle et optimiser le contrôle. Grâce à sa construction innovante, il offre des trajectoires précises et une distance exceptionnelle. La forme aérodynamique réduisant la traînée favorise des coups plus longs et plus droits. Le Cobra Speedzone King SZ est le choix idéal pour les golfeurs en quête de performances de pointe, offrant une qualité supérieure pour un jeu optimal.", 280, null, 16, 279.99m, DateTime.Today.AddDays(-190), 219.97m);
            Product productBBA2940TMD = AddProduct(builder, "BBA2940TMD", "Taylormade Stealth PLUS+ - Bois d'allée", "Taylormade", "Stealth PLUS+", "~/img/products/BBA2940TMD.jpg", "Le Taylormade Stealth PLUS+: Puissance et précision.", "Découvrez le bois d'allée Taylormade Stealth PLUS+, une révolution de puissance et de précision pour votre jeu de golf. Ce bois d'allée est conçu avec des technologies de pointe pour optimiser la vitesse de la balle, offrant des coups plus longs et plus droits. La construction innovante en acier au carbone assure une stabilité exceptionnelle, tandis que le design aérodynamique réduit la traînée pour des performances de pointe. Le Taylormade Stealth PLUS+ est le choix idéal pour les golfeurs en quête d'une qualité supérieure, offrant des trajectoires précises et des distances impressionnantes sur le parcours. Repoussez les limites de votre jeu avec ce bois d'allée exceptionnel.", 285, null, 19, 439.97m, DateTime.Today.AddDays(-215));
            Product productBBA6254CAL = AddProduct(builder, "BBA6254CAL", "Callaway Epic Speed - Bois d'allée", "Callaway", "Epic Speed", "~/img/products/BBA6254CAL.jpg", "Le Callaway Epic Speed: Vitesse exceptionnelle.", "Découvrez le bois d'allée Callaway Epic Speed, un club conçu pour offrir une vitesse exceptionnelle et des performances de haut niveau sur le parcours. Ce bois d'allée intègre des innovations technologiques pour maximiser la vitesse de la balle, vous permettant d'atteindre des distances incroyables. Sa construction de pointe assure une stabilité et un contrôle optimaux, tandis que le design aérodynamique réduit la traînée pour des coups plus longs et plus droits. Le Callaway Epic Speed est le choix idéal pour les golfeurs qui cherchent à repousser les limites de leur jeu, offrant une qualité exceptionnelle et des trajectoires précises. Dominez le parcours avec ce bois d'allée révolutionnaire.", 285, null, 11, 299.87m, DateTime.Today.AddDays(-238));
            Product productBHY2367CAL = AddProduct(builder, "BHY2367CAL", "Callaway Paradym - Hybride", "Callaway", "Paradym", "~/img/products/BHY2367CAL.jpg", "Le Callaway Paradym: Polyvalence et précision.", "Découvrez le bâton hybride Callaway Paradym, un club qui allie polyvalence et précision pour améliorer votre jeu de golf. Ce hybride est conçu avec des technologies avancées pour offrir une combinaison parfaite de distance et de contrôle. Sa construction innovante permet d'optimiser la vitesse de la balle, offrant des coups longs et précis. Le poids stratégiquement placé favorise un centre de gravité bas pour des trajectoires précises et un contrôle optimal. Le Callaway Paradym est idéal pour les golfeurs de tous niveaux, offrant une qualité supérieure pour des performances exceptionnelles sur le parcours. Améliorez votre jeu avec ce bâton hybride polyvalent et fiable.", 265, null, 20, 419.99m, DateTime.Today.AddDays(-239));
            Product productBHY0437PNG = AddProduct(builder, "BHY0437PNG", "Ping G430 - Hybride", "Ping", "G430", "~/img/products/BHY0437PNG.jpg", "Le Ping G430: Puissance et contrôle.", "Découvrez le bâton hybride Ping G430, un club qui allie puissance et contrôle pour améliorer votre jeu de golf. Conçu avec des avancées technologiques, ce hybride offre des performances exceptionnelles sur le parcours. Sa construction innovante permet d'optimiser la vitesse de la balle, offrant des coups longs et précis. Le poids stratégiquement placé favorise un centre de gravité bas pour des trajectoires précises et un contrôle optimal. Le Ping G430 est le choix idéal pour les golfeurs de tous niveaux, offrant une qualité supérieure pour des performances inégalées. Repoussez les limites de votre jeu avec ce bâton hybride fiable et performant.", 270, null, 9, 439.99m, DateTime.Today.AddDays(-315));
            Product productBHY8741WIL = AddProduct(builder, "BHY8741WIL", "Wilson D7 - Hybride", "Wilson", "D7", "~/img/products/BHY8741WIL.jpg", "Le Wilson D7: Puissance et précision.", "Découvrez le bâton hybride Wilson D7, un club qui redéfinit la puissance et la précision sur le parcours de golf. Grâce à des avancées technologiques de pointe, ce hybride offre des performances exceptionnelles. Sa construction innovante permet d'optimiser la vitesse de la balle, offrant des coups longs et précis. Le poids stratégiquement placé favorise un centre de gravité bas pour des trajectoires précises et un contrôle optimal. Le Wilson D7 est l'outil idéal pour les golfeurs de tous niveaux, offrant une qualité supérieure pour des performances inégalées. Améliorez votre jeu avec ce bâton hybride fiable et performant.", 260, null, 2, 199.99m, DateTime.Today.AddDays(-349), 119.87m);
            Product productBCO7236COB = AddProduct(builder, "BCO7236COB", "Cobra Snakebite - Cocheur", "Cobra", "Snakebite", "~/img/products/BCO7236COB.jpg", "Le Cobra Snakebite: Précision et contrôle.", "Découvrez le bâton cocheur Cobra Snakebite, un chef-d'œuvre de précision et de contrôle pour votre jeu de golf. Conçu avec des technologies avancées, ce cocheur offre une combinaison parfaite de distance et de maîtrise sur le parcours. Sa construction innovante permet d'optimiser la trajectoire de la balle, offrant des coups précis et contrôlés. La face texturée \"Snakebite Groove\" assure une adhérence maximale pour des spins exceptionnels, même depuis les zones les plus délicates du parcours. Le Cobra Snakebite est l'outil idéal pour les golfeurs exigeants, offrant une qualité supérieure et des performances inégalées. Révélez tout votre potentiel avec ce bâton cocheur exceptionnel.", 230, null, 18, 179.99m, DateTime.Today.AddDays(-184));
            Product productBCO3012TMD = AddProduct(builder, "BCO3012TMD", "Taylormade Grind 4 - Cocheur", "Taylormade", "Grind 4", "~/img/products/BCO3012TMD.jpg", "Le Taylormade Grind 4: Précision inégalée.", "Découvrez le bâton cocheur Taylormade Grind 4, un outil de précision conçu pour les golfeurs les plus exigeants. Ce cocheur intègre des technologies de pointe pour garantir une maîtrise inégalée sur le parcours. Sa construction innovante offre une sensation exceptionnelle et des performances fiables, que vous soyez dans le bunker ou sur le green. Les rainures ZTP-17 assurent un spin optimal pour un contrôle total de la balle, tandis que la face fraisée milled grind offre une adhérence maximale. Le Taylormade Grind 4 est le choix parfait pour les golfeurs en quête de qualité supérieure, offrant des coups précis et une confiance totale dans leur jeu court. Maîtrisez chaque coup avec ce bâton cocheur d'exception.", 230, null, 14, 239.99m, DateTime.Today.AddDays(-230), 209.87m);
            Product productBCO1992CLE = AddProduct(builder, "BCO1992CLE", "Cleveland RTX Full-Face - Cocheur", "Cleveland", "RTX Full-Face", "~/img/products/BCO1992CLE.jpg", "Le Cleveland RTX Full-Face: Contrôle absolu.", "Découvrez le bâton cocheur Cleveland RTX Full-Face, un chef-d'œuvre de précision pour votre jeu de golf. Conçu avec des technologies avancées, ce cocheur offre un contrôle absolu sur le parcours. Sa construction innovante assure une adhérence maximale grâce à la face Full-Face, vous permettant de réaliser des coups précis et contrôlés depuis n'importe quel angle. Les rainures ZipCore favorisent un spin optimal pour des performances exceptionnelles sur le green. Le Cleveland RTX Full-Face est l'outil idéal pour les golfeurs exigeants, offrant une qualité supérieure et une confiance totale dans leur jeu court. Maîtrisez chaque coup avec ce sublime bâton cocheur.", 230, null, 5, 199.99m, DateTime.Today.AddDays(-219), 179.87m);
            Product productBFD9218PNG = AddProduct(builder, "BFD9218PNG", "Ping PLD Milled - Fer droit", "Ping", "PLD Milled", "~/img/products/BFD9218PNG.jpg", "Le Ping PLD Milled: Précision et performance.", "Découvrez le bâton fer droit Ping PLD Milled, une œuvre d'art de la précision et de la performance pour votre jeu de golf. Conçu avec une fabrication de pointe, ce fer droit offre une précision exceptionnelle et des performances inégalées sur le parcours. La face fraisée milled assure une surface plane pour un impact précis, tandis que le centre de gravité optimal favorise des trajectoires précises et une distance maximale. Le Ping PLD Milled est l'outil idéal pour les golfeurs exigeants, offrant une qualité supérieure et une confiance totale dans leur jeu. Maîtrisez chaque coup avec ce fer droit d'exception qui repousse les limites de vos performances.", 350, null, 5, 599.99m, DateTime.Today.AddDays(-363));
            Product productBFD8432PNG = AddProduct(builder, "BFD8432PNG", "Ping Sigma 2 Fetch - Fer droit", "Ping", "Sigma 2 Fetch", "~/img/products/BFD8432PNG.jpg", "Le Ping Sigma 2 Fetch: Contrôle et précision.", "Découvrez le bâton fer droit Ping Sigma 2 Fetch, un club qui allie contrôle et précision pour améliorer votre jeu de golf. Conçu avec des technologies avancées, ce fer droit offre une combinaison parfaite de distance et de maîtrise sur le parcours. La face fraisée milled favorise une surface plane pour des coups précis, tandis que le centre de gravité optimisé assure des trajectoires précises et une distance maximale. Le design à deux disques vous permet de relever facilement la balle du green, offrant une polyvalence exceptionnelle dans votre jeu court. Le Ping Sigma 2 Fetch est l'outil idéal pour les golfeurs de tous niveaux, offrant une qualité supérieure et une confiance totale dans leur jeu. Maîtrisez chaque coup avec ce fer droit d'exception.", 353, null, 21, 299.99m, DateTime.Today.AddDays(-363), 239.87m);
            Product productBFD3983COB = AddProduct(builder, "BFD3983COB", "Cobra 3D Printed Grandsport-35 - Fer droit", "Cobra", "3D Printed Grandsport-35", "~/img/products/BFD3983COB.jpg", "Le Cobra 3D Printed Grandsport-35: Innovation et précision.", "Découvrez le bâton fer droit Cobra 3D Printed Grandsport-35, un chef-d'œuvre d'innovation et de précision pour votre jeu de golf. Conçu avec des technologies de pointe, ce fer droit repousse les limites de la performance sur le parcours. La technologie d'impression 3D révolutionnaire crée une face ultra-mince pour des coups précis et puissants. Le centre de gravité optimal assure des trajectoires précises et une distance maximale, tandis que la structure en carbone favorise un toucher exceptionnel. Le Cobra 3D Printed Grandsport-35 est l'outil idéal pour les golfeurs exigeants, offrant une qualité supérieure et une confiance totale dans leur jeu. Maîtrisez chaque coup avec ce fer droit exceptionnel.", 347, null, 16, 449.99m, DateTime.Today.AddDays(-298), 339.97m);
            Product productBAL7290TMD = AddProduct(builder, "BAL7290TMD", "Taylormade Soft Response - Balles", "Taylormade", "Soft Response", "~/img/products/BAL7290TMD.jpg", "Les Taylormade Soft Response: Pour des tirs précis et un excellent contrôle.", "Les balles de golf Taylormade Soft Response offrent une combinaison parfaite de douceur et de performances. Grâce à leur noyau souple et à leur enveloppe réactive, elles offrent une sensation exceptionnelle au toucher tout en garantissant une distance impressionnante sur le parcours. Conçues pour les golfeurs de tous niveaux, ces balles offrent un excellent contrôle sur le green, ce qui vous permet de peaufiner votre jeu court. Que vous soyez un golfeur amateur ou expérimenté, les Taylormade Soft Response sont un choix fiable pour améliorer vos performances sur le terrain.", 46, null, 19, 34.99m, DateTime.Today.AddDays(-322));
            Product productBAL5451BRI = AddProduct(builder, "BAL5451BRI", "Brigestone e12 Contact - Balles", "Brigestone", "e12 Contact", "~/img/products/BAL5451BRI.jpg", "Les Bridgestone e12 Contact: Distance exceptionnelle et réactivité optimale.", "Les balles de golf Bridgestone e12 Contact sont conçues pour offrir des performances exceptionnelles sur le parcours. Grâce à leur noyau innovant et leur enveloppe souple, elles assurent une distance impressionnante tout en conservant un excellent contrôle sur le green. Le motif de contact de la balle améliore la réactivité sur les coups roulés, vous permettant de maximiser la précision de vos putts. Ces balles sont idéales pour les golfeurs de tous niveaux à la recherche d'un équilibre parfait entre distance, contrôle et sensation. Choisissez les Bridgestone e12 Contact pour optimiser votre jeu golfique.", 47, null, 3, 44.99m, DateTime.Today.AddDays(-329), 29.87m);
            Product productBAL9823WIL = AddProduct(builder, "BAL9823WIL", "Wilson Duo Soft - Balles", "Wilson", "Duo Soft", "~/img/products/BAL9823WIL.jpg", "Les Wilson Duo Soft: Un toucher exceptionnel sur le green.", "Les balles de golf Wilson Duo Soft sont le choix idéal pour les golfeurs à la recherche d'une sensation exceptionnelle sur le green. Leur noyau souple assure un toucher doux et une réactivité optimale, ce qui vous permet de contrôler vos putts avec précision. De plus, ces balles offrent une distance respectable, les rendant adaptées à un large éventail de joueurs, des débutants aux joueurs expérimentés. Les Duo Soft de Wilson sont conçues pour améliorer votre jeu court, vous permettant de gagner en confiance sur les greens exigeants. Optez pour la qualité Wilson pour des performances golfiques exceptionnelles.", 47, null, 13, 29.99m, DateTime.Today.AddDays(-287));
            Product productVPO3595UND = AddProduct(builder, "VPO3595UND", "Under Armour Men's Playoff 3.0 - Polo", "Under Armour", "Men's Playoff 3.0", "~/img/products/VPO3595UND.jpg", "Le polo Under Armour Men's Playoff 3.0: Confort exceptionnel sur le terrain.", "Le polo Under Armour Men's Playoff 3.0 est le choix parfait pour les golfeurs à la recherche de confort et de style sur le terrain. Fabriqué avec des matériaux de haute qualité, ce polo offre une excellente respirabilité et évacue l'humidité, vous permettant de rester au sec pendant toute la partie. Sa coupe ajustée et son design élégant en font un vêtement polyvalent qui peut être porté sur et hors du terrain de golf. Les détails de finition, tels que le col côtelé et le logo UA, ajoutent une touche de sophistication. Améliorez votre jeu et votre style avec le polo Playoff 3.0 d'Under Armour.", null, null, 2, 79.99m, DateTime.Today.AddDays(-223));
            Product productVPO7518NIK = AddProduct(builder, "VPO7518NIK", "Nike Men's Dri-FIT Victory - Polo", "Nike", "Men's Playoff 3.0", "~/img/products/VPO7518NIK.jpg", "Le polo Nike Men's Dri-FIT Victory: Style et performance sur le green.", "Le polo Nike Men's Dri-FIT Victory est le choix idéal pour les golfeurs exigeants. Fabriqué avec la technologie Dri-FIT de Nike, il évacue l'humidité pour vous garder au sec, même par temps chaud. Sa coupe athlétique offre une grande liberté de mouvement, tandis que son design épuré et élégant vous permet de jouer avec style. Le col côtelé et les boutons lui confèrent une touche de sophistication. Que vous soyez sur le green ou en dehors, ce polo est un incontournable de la garde-robe golfique. Optez pour le confort et l'élégance avec le polo Nike Dri-FIT Victory.", null, null, 9, 69.99m, DateTime.Today.AddDays(-303), 59.87m);
            Product productVPO9284PUM = AddProduct(builder, "VPO9284PUM", "Puma Men's MATTR Pastimes - Polo", "Puma", "Men's MATTR Pastimes", "~/img/products/VPO9284PUM.jpg", "Le polo Puma Men's MATTR Pastimes: Confort et style sur le parcours.", "Le polo Puma Men's MATTR Pastimes est conçu pour les golfeurs à la recherche d'un mélange parfait entre confort et style. Fabriqué avec des matériaux de haute qualité, il offre une respirabilité exceptionnelle et une gestion efficace de l'humidité, vous maintenant au sec même par temps chaud. La coupe classique et les détails de finition élégants ajoutent une touche de sophistication à votre tenue de golf. Que vous jouiez sur le green ou que vous vous détendiez après la partie, ce polo vous assure un look raffiné. Optez pour la qualité Puma et redéfinissez votre style sur le parcours de golf.", null, null, 21, 109.99m, DateTime.Today.AddDays(-190));
            Product productVPA3732UND = AddProduct(builder, "VPA3732UND", "Under Armour Men's Drive Jogger - Pantalon", "Under Armour", "Men's Drive Jogger", "~/img/products/VPA3732UND.jpg", "Les pantalons Under Armour Men's Drive Jogger: Style et performance sur le parcours.", "Les pantalons Under Armour Men's Drive Jogger sont conçus pour les golfeurs exigeants à la recherche de confort et d'élégance. Fabriqués avec des matériaux de haute qualité, ces pantalons offrent une excellente respirabilité et extensibilité pour une liberté de mouvement optimale. Leur coupe ajustée et leur design élégant en font un choix polyvalent, parfait pour le golf et les activités décontractées en dehors du parcours. Les détails de finition, tels que les poignets côtelés et la ceinture élastique, ajoutent une touche de sophistication. Optez pour le confort et le style sur le parcours avec les pantalons Drive Jogger d'Under Armour.", null, null, 12, 109.99m, DateTime.Today.AddDays(-243), 84.87m);
            Product productVPA7388FOO = AddProduct(builder, "VPA7388FOO", "Footjoy Men's HYPR Golf Jogger - Pantalon", "Footjoy", "Men's HYPR Golf Jogger", "~/img/products/VPA7388FOO.jpg", "Les pantalons Footjoy Men's HYPR Golf Jogger: Style et confort en tout temps.", "Les pantalons Footjoy Men's HYPR Golf Jogger sont conçus pour les golfeurs modernes qui recherchent un mélange de style et de performance. Fabriqués avec des matériaux de haute qualité, ces pantalons offrent une grande liberté de mouvement, ce qui en fait un choix idéal pour le golf. Leur coupe élégante et leur design épuré ajoutent une touche de sophistication à votre tenue de golf. La ceinture élastique assure un ajustement confortable, tandis que les poignets côtelés garantissent un ajustement parfait. Ces pantalons sont parfaits pour jouer au golf tout en ayant fière allure. Optez pour l'élégance et la performance avec les pantalons Footjoy HYPR Golf Jogger.", null, null, 19, 159.99m, DateTime.Today.AddDays(-263));
            Product productVPA1989PUM = AddProduct(builder, "VPA1989PUM", "Puma Men's Jackpot 5 - Pantalon", "Puma", "Men's Jackpot 5", "~/img/products/VPA1989PUM.jpg", "Les pantalons Puma Men's Jackpot 5: Style et confort sur le parcours.", "Les pantalons Puma Men's Jackpot 5 sont un choix élégant et fonctionnel pour les golfeurs exigeants. Fabriqués avec des matériaux de haute qualité, ils offrent un ajustement confortable et une grande liberté de mouvement pour votre swing. La coupe ajustée et le design moderne ajoutent une touche de sophistication à votre tenue de golf. La ceinture élastique et les poches pratiques complètent ces pantalons. Que vous jouiez une partie de golf ou que vous passiez du temps au clubhouse, les pantalons Jackpot 5 vous offrent un style inégalé et un confort optimal. Optez pour la qualité et l'élégance sur le parcours avec Puma.", null, null, 13, 119.99m, DateTime.Today.AddDays(-290), 59.87m);
            Product productVCH8350FOO = AddProduct(builder, "VCH8350FOO", "Footjoy Men's Hyperflex Spiked - Chaussure", "Footjoy", "Men's Hyperflex Spiked", "~/img/products/VCH8350FOO.jpg", "Les chaussures Footjoy Men's Hyperflex Spiked: Confort exceptionnel sur le parcours.", "Les chaussures Footjoy Men's Hyperflex Spiked sont conçues pour offrir un confort exceptionnel aux golfeurs. Grâce à leur conception innovante, elles offrent un ajustement sûr et une grande stabilité, aidant ainsi à améliorer votre performance sur le parcours. La technologie Hyperflex garantit une flexibilité maximale pour un mouvement naturel du pied, tandis que les crampons offrent une adhérence exceptionnelle, même dans des conditions humides. La semelle intermédiaire confortable procure un amorti supplémentaire, réduisant la fatigue pendant les longues parties. Ces chaussures combinent style et fonctionnalité, en faisant un choix idéal pour tout golfeur à la recherche de performance et de confort.", null, null, 5, 229.99m, DateTime.Today.AddDays(-256));
            Product productVCH1873UND = AddProduct(builder, "VCH1873UND", "Under Armour Men's HOVR Drive Spikeless - Chaussure", "Under Armour", "Men's HOVR Drive Spikeless", "~/img/products/VCH1873UND.jpg", "Les chaussures Under Armour Men's HOVR Drive Spikeless: Synonymes de confort sur le parcours.", "Les chaussures Under Armour Men's HOVR Drive Spikeless sont le choix parfait pour les golfeurs à la recherche de confort et de style. Avec leur conception spikeless, elles offrent une adhérence fiable sans endommager le parcours. La technologie HOVR garantit un amorti optimal, réduisant la fatigue pendant les longues parties de golf. La tige légère et respirante assure une aération constante pour garder vos pieds au sec. Ces chaussures combinent une esthétique élégante avec une performance exceptionnelle, vous permettant de vous concentrer sur votre jeu sans compromettre le confort de vos pieds. Affrontez le parcours avec confiance grâce à ces chaussures de golf de qualité.", null, null, 13, 174.99m, DateTime.Today.AddDays(-230), 149.87m);
            Product productVCH9748NIK = AddProduct(builder, "VCH9748NIK", "Nike Air Max 270 G Spikeless - Chaussure", "Nike", "Air Max 270 G Spikeless", "~/img/products/VCH9748NIK.jpg", "Les chaussures Nike Air Max 270 G Spikeless: Style emblématique sur le parcours.", "Les chaussures Nike Air Max 270 G Spikeless combinent un design classique avec des performances de pointe sur le parcours de golf. Inspirées des légendaires baskets Air Max, elles offrent un confort exceptionnel grâce à l'amorti Air Max dans la semelle intermédiaire. La semelle spikeless assure une traction fiable sans endommager le gazon, tandis que la tige respirante garde vos pieds au sec par temps chaud. Ces chaussures sont idéales pour les golfeurs qui veulent allier style et performance. Marchez sur le parcours avec confiance et montrez votre amour pour la mode, tout en améliorant votre jeu grâce à ces chaussures polyvalentes.", null, null, 8, 194.99m, DateTime.Today.AddDays(-159));
            Product productSSP2922TIT = AddProduct(builder, "SSP2922TIT", "Titleist Players 4 StaDry - Sac sur pied", "Titleist", "Players 4 StaDry", "~/img/products/SSP2922TIT.jpg", "Le sac Titleist Players 4 StaDry: Allie style et fonctionnalité sur le parcours.", "Le sac sur pied Titleist Players 4 StaDry est conçu pour offrir une performance exceptionnelle sur le parcours de golf. Fabriqué avec des matériaux imperméables de haute qualité, il garde vos clubs et vos équipements au sec par temps humide. Avec un design élégant et des fonctionnalités intelligentes, ce sac offre de nombreuses poches de rangement, un support solide, des sangles rembourrées pour un transport confortable, et des diviseurs pour organiser vos clubs. Que vous marchiez ou que vous utilisiez une voiturette, ce sac vous accompagnera avec style et fonctionnalité. C'est le choix idéal pour les golfeurs soucieux de la qualité et du design sur le parcours.", 1700, null, 9, 419.99m, DateTime.Today.AddDays(-189));
            Product productSSP2323NIK = AddProduct(builder, "SSP2323NIK", "Nike Jordan Fadeaway - Sac sur pied", "Nike", "Jordan Fadeaway", "~/img/products/SSP2323NIK.jpg", "Le sac Nike Jordan Fadeaway: Polyvalent et stylisé.", "Le sac sur pied Nike Jordan Fadeaway est conçu pour combiner style et performance sur le parcours de golf. Arborant le design emblématique de Jordan, ce sac est idéal pour les golfeurs à la recherche d'un look distinctif. Il offre de nombreuses poches pour le rangement de vos clubs, de vos balles et de vos accessoires, ainsi qu'un support solide et des sangles rembourrées pour un transport confortable. Les diviseurs intégrés vous aident à garder vos clubs organisés et protégés. Que vous marchiez ou que vous utilisiez une voiturette, ce sac vous permettra de jouer avec style et praticité, reflétant votre passion pour le golf et la mode Jordan.", 1900, null, 4, 439.99m, DateTime.Today.AddDays(-283), 307.87m);
            Product productSSP8981CAL = AddProduct(builder, "SSP8981CAL", "Callaway X-Carry - Sac sur pied", "Callaway", "X-Carry", "~/img/products/SSP8981CAL.jpg", "Le sac Callaway X-Carry: Légèreté et polyvalence pour une journée de golf sans soucis.", "Le sac sur pied Callaway X-Carry est le compagnon idéal pour les golfeurs qui recherchent la combinaison parfaite de légèreté et de fonctionnalité. Il offre une conception ergonomique avec un grand espace de rangement, des poches pratiques pour les accessoires, et un support solide pour maintenir vos clubs en sécurité. Ses sangles rembourrées et son système de transport à double sangle garantissent un confort optimal pendant toute la partie. Les diviseurs pleine longueur maintiennent vos clubs organisés et protégés. Que vous préfériez marcher sur le parcours ou utiliser une voiturette, le Callaway X-Carry est prêt à répondre à vos besoins et à améliorer votre expérience de golf.", 1800, null, 13, 259.99m, DateTime.Today.AddDays(-233));
            Product productSPC7230CAL = AddProduct(builder, "SPC7230CAL", "Callaway Org 14 - Sac pour chariot", "Callaway", "Org 14", "~/img/products/SPC7230CAL.jpg", "Le sac Callaway Org 14: Rangement intelligent et une grande capacité pour une organisation optimale sur le parcours.", "Le sac pour chariot Callaway Org 14 est conçu pour répondre aux besoins des golfeurs les plus exigeants en matière d'organisation et de capacité de rangement. Doté de 14 diviseurs pleine longueur, il garantit la protection de chaque club et une organisation optimale. Les nombreuses poches, y compris des poches isothermes, vous permettent de ranger vos accessoires, vos balles, vos vêtements et bien plus encore. Le sac est également équipé d'un porte-parapluie, d'un porte-serviette et d'un système de fixation pour votre chariot. La qualité de fabrication et les détails astucieux font du Callaway Org 14 un choix exceptionnel pour les golfeurs souhaitant une expérience de golf sans tracas et bien organisée.", 2200, null, 11, 349.99m, DateTime.Today.AddDays(-214));
            Product productSPC2943TMD = AddProduct(builder, "SPC2943TMD", "Taylormade Cart Lite - Sac pour chariot", "Taylormade", "Cart Lite", "~/img/products/SPC2943TMD.jpg", "Le Taylormade Cart Lite: Légèreté et de style sur le parcours.", "Le sac pour chariot Taylormade Cart Lite est un choix idéal pour les golfeurs à la recherche de légèreté et de praticité. Doté de 14 diviseurs pleine longueur, il assure une organisation soignée de vos clubs tout en les protégeant. Les nombreuses poches offrent un espace généreux pour vos accessoires, vos vêtements et vos effets personnels. Ce sac comprend également un porte-parapluie, un porte-serviette et une poche isotherme pour garder vos boissons fraîches. Avec son design élégant et ses détails de qualité, le Taylormade Cart Lite allie style et fonctionnalité pour une expérience de golf exceptionnelle.", 2200, null, 4, 339.99m, DateTime.Today.AddDays(-189), 299.87m);
            Product productSPC1999WIL = AddProduct(builder, "SPC1999WIL", "Wilson Lite II - Sac pour chariot", "Wilson", "Lite II", "~/img/products/SPC1999WIL.jpg", "Le sac Wilson Lite II: L'allié parfait pour transporter vos clubs et équipements sur le parcours.", "Le sac pour chariot Wilson Lite II est conçu pour offrir une commodité maximale aux golfeurs. Il dispose de 14 diviseurs pleine longueur pour une organisation impeccable de vos clubs, réduisant les risques de rayures et d'emmêlement. Avec de nombreuses poches, y compris une poche isotherme pour vos boissons et une poche imperméable, vous pouvez emporter tout ce dont vous avez besoin. Les détails pratiques comme le porte-serviette, le porte-parapluie et la poignée intégrée facilitent le transport. Ce sac combine fonctionnalité, style et durabilité pour une journée sur le parcours sans souci.", 2200, null, 6, 169.99m, DateTime.Today.AddDays(-329));
            Product productATE7832BUS = AddProduct(builder, "ATE7832BUS", "Bushnell Pro X7 - Télémètre", "Bushnell", "Pro X7", "~/img/products/ATE7832BUS.jpg", "Le Bushnell Pro X7: L'outil ultime pour les golfeurs sérieux.", "Le télémètre Bushnell Pro X7 est un appareil de haute précision conçu pour les golfeurs exigeants. Il offre une mesure rapide et précise de la distance jusqu'au drapeau, aux obstacles et aux points de repère, vous permettant de prendre des décisions éclairées sur le parcours. Doté de la technologie E.S.P. (Extreme Speed Precision), il garantit une précision inégalée, même sur des parcours vallonnés. Avec son grossissement 7x, l'écran lumineux et les lentilles multicouches, il offre une excellente visibilité dans toutes les conditions. Le Pro X7 est étanche et durable, ce qui en fait un compagnon fiable sur le parcours, quelles que soient les conditions météorologiques.", 454, null, 2, 549.99m, DateTime.Today.AddDays(-284), 199.87m);
            Product productATE5376BUS = AddProduct(builder, "ATE5376BUS", "Bushnell Tour V6 - Télémètre", "Bushnell", "Tour V6", "~/img/products/ATE5376BUS.jpg", "Le Bushnell Tour V6: Précision et performance sur le parcours.", "Le télémètre Bushnell Tour V6 est un outil incontournable pour les golfeurs soucieux de la précision. Grâce à sa technologie PinSeeker avec la technologie JOLT, il verrouille rapidement la cible et émet une vibration pour confirmer la mesure de la distance jusqu'au drapeau. Avec une portée allant jusqu'à 400 mètres et un grossissement de 6x, vous obtenez des lectures précises, même sur les parcours les plus longs. Compact et facile à manipuler, le Tour V6 se glisse facilement dans votre poche ou votre sac de golf. Ne laissez plus la distance vous surprendre sur le parcours, choisissez le Bushnell Tour V6 pour des tirs précis et réussis.", 447, null, 7, 439.99m, DateTime.Today.AddDays(-285));
            Product productATE0012VCA = AddProduct(builder, "ATE0012VCA", "Voice Caddie SL2 Hybrid - Télémètre", "Voice Caddie", "SL2 Hybrid", "~/img/products/ATE0012VCA.jpg", "Le Voice Caddie SL2 Hybrid : Votre caddy vocal personnel.", "Le télémètre Voice Caddie SL2 Hybrid révolutionne votre expérience de golf en vous offrant un caddy vocal personnel. Il utilise une technologie avancée pour mesurer avec précision la distance jusqu'au drapeau, aux obstacles et aux points de repère. Vous pouvez obtenir des informations vocales en temps réel, ce qui vous permet de rester concentré sur votre jeu sans avoir à regarder un écran. Avec une portée allant jusqu'à 800 mètres, le SL2 Hybrid est adapté à tous les parcours. Compact et facile à utiliser, il se fixe facilement à votre sac ou à votre ceinture. Laissez le Voice Caddie SL2 Hybrid améliorer vos performances sur le parcours et vous guider vers des coups gagnants.", 412, null, 17, 949.99m, DateTime.Today.AddDays(-196), 799.87m);
            Product productAGP5432SKY = AddProduct(builder, "AGP5432SKY", "Skygolf SkyCaddie SGX-W - GPS", "Skygolf", "SkyCaddie SGX-W", "~/img/products/AGP5432SKY.jpg", "Le Skygolf SkyCaddie SGX-W: l'outil essentiel pour des coups précis.", "Le GPS Skygolf SkyCaddie SGX-W est conçu pour améliorer votre jeu de golf. Il offre des informations précises sur les distances aux obstacles, aux bunkers, et aux greens, vous aidant à prendre des décisions éclairées sur le parcours. Son écran couleur tactile est facile à lire, même en plein soleil, et il propose des vues détaillées des parcours pour une navigation aisée. Avec une grande autonomie de batterie, ce GPS est parfait pour une longue journée sur le terrain. Ajoutez un avantage stratégique à votre jeu avec le Skygolf SkyCaddie SGX-W et améliorez vos performances sur le parcours.", 159, null, 13, 449.99m, DateTime.Today.AddDays(-106), 149.87m);
            Product productAGP9842GAR = AddProduct(builder, "AGP9842GAR", "Garmin Approach G6 - GPS", "Garmin", "Approach G6", "~/img/products/AGP9842GAR.jpg", "Le Garmin Approach G6: La précision au service de votre swing.", "Le GPS Garmin Approach G6 est l'outil essentiel pour les golfeurs soucieux de leur performance. Doté d'un écran couleur lumineux et lisible en plein soleil, il vous fournit des données précises sur les distances, les obstacles et les greens. Il propose des cartes de parcours détaillées pour une navigation aisée. Vous pouvez même mesurer vos coups et analyser votre jeu. Avec une conception légère et compacte, le Garmin Approach G6 se glisse facilement dans votre poche, vous permettant de l'emporter partout. Améliorez votre jeu et atteignez de nouveaux sommets sur le parcours grâce à cet outil de précision.", 148, null, 5, 249.99m, DateTime.Today.AddDays(-139), 149.87m);
            Product productAGP3890SHS = AddProduct(builder, "AGP3890SHS", "Shot Scope X5 - GPS", "Shot Scope", "X5", "~/img/products/AGP3890SHS.jpg", "Le Shot Scope X5: Suivez et améliorez votre jeu.", "Le GPS Shot Scope X5 est un compagnon incontournable pour les golfeurs sérieux. Il offre une multitude de fonctionnalités pour améliorer votre jeu. Enregistrez automatiquement chaque coup, suivez vos statistiques, mesurez les distances, et obtenez des informations sur les obstacles. Grâce à son écran couleur lisible en plein soleil, il est facile à consulter sur le parcours. De plus, il est également élégant et confortable à porter au quotidien. Le Shot Scope X5 vous donne un avantage concurrentiel en fournissant des données de jeu précises et en vous aidant à prendre des décisions éclairées sur le parcours.", 190, null, 19, 399.99m, DateTime.Today.AddDays(-83));
            #endregion

            // Ajouter les produits à leurs catégories.
            #region Produits dans catégorie
            AddProductToCategory(builder, boisDeDepart, productBBD3845TMD);
            AddProductToCategory(builder, boisDeDepart, productBBD4058CAL);
            AddProductToCategory(builder, boisDeDepart, productBBD8943TMD);
            AddProductToCategory(builder, fers, productBFE1849CAL);
            AddProductToCategory(builder, fers, productBFE4924CLE);
            AddProductToCategory(builder, fers, productBFE5421CAL);
            AddProductToCategory(builder, boisAllee, productBBA2940TMD);
            AddProductToCategory(builder, boisAllee, productBBA6254CAL);
            AddProductToCategory(builder, boisAllee, productBBA8492COB);
            AddProductToCategory(builder, hybrides, productBHY0437PNG);
            AddProductToCategory(builder, hybrides, productBHY2367CAL);
            AddProductToCategory(builder, hybrides, productBHY8741WIL);
            AddProductToCategory(builder, cocheurs, productBCO1992CLE);
            AddProductToCategory(builder, cocheurs, productBCO3012TMD);
            AddProductToCategory(builder, cocheurs, productBCO7236COB);
            AddProductToCategory(builder, fersDroits, productBFD3983COB);
            AddProductToCategory(builder, fersDroits, productBFD8432PNG);
            AddProductToCategory(builder, fersDroits, productBFD9218PNG);
            AddProductToCategory(builder, balles, productBAL5451BRI);
            AddProductToCategory(builder, balles, productBAL7290TMD);
            AddProductToCategory(builder, balles, productBAL9823WIL);
            AddProductToCategory(builder, polos, productVPO3595UND);
            AddProductToCategory(builder, polos, productVPO7518NIK);
            AddProductToCategory(builder, polos, productVPO9284PUM);
            AddProductToCategory(builder, pantalons, productVPA1989PUM);
            AddProductToCategory(builder, pantalons, productVPA3732UND);
            AddProductToCategory(builder, pantalons, productVPA7388FOO);
            AddProductToCategory(builder, chaussures, productVCH1873UND);
            AddProductToCategory(builder, chaussures, productVCH8350FOO);
            AddProductToCategory(builder, chaussures, productVCH9748NIK);
            AddProductToCategory(builder, sacsSurPied, productSSP2323NIK);
            AddProductToCategory(builder, sacsSurPied, productSSP2922TIT);
            AddProductToCategory(builder, sacsSurPied, productSSP8981CAL);
            AddProductToCategory(builder, sacsPourChariot, productSPC1999WIL);
            AddProductToCategory(builder, sacsPourChariot, productSPC2943TMD);
            AddProductToCategory(builder, sacsPourChariot, productSPC7230CAL);
            AddProductToCategory(builder, telemetres, productATE0012VCA);
            AddProductToCategory(builder, telemetres, productATE5376BUS);
            AddProductToCategory(builder, telemetres, productATE7832BUS);
            AddProductToCategory(builder, gps, productAGP3890SHS);
            AddProductToCategory(builder, gps, productAGP9842GAR);
            AddProductToCategory(builder, gps, productAGP5432SKY);
            #endregion

            // Ajouter les commandes
            #region Ajouter une commande
            AddOrder(builder, address, adminUser, new List<Product> { productBBD3845TMD, productSSP2323NIK }, "AH83HDKV73HFMV82", "Admin", "Admin", OrderStatus.Confirmed, "admin@ballsrus.ca", "1234567890", 123.99m);
            AddOrder(builder, address, adminUser, new List<Product> { productATE7832BUS, productAGP5432SKY }, "JHSDHFJH2U98EFHJ", "Admin", "Admin", OrderStatus.Confirmed, "admin@ballsrus.ca", "1234567890", 143.99m);
            AddOrder(builder, address, adminUser, new List<Product> { productSSP8981CAL, productVCH1873UND }, "KJHMZXHHJHDFS823", "Admin", "Admin", OrderStatus.Confirmed, "admin@ballsrus.ca", "1234567890", 436.99m);
            AddOrder(builder, address, adminUser, new List<Product> { productVCH1873UND, productVCH1873UND }, "MXBJSAJ128B7NN22", "Admin", "Admin", OrderStatus.Confirmed, "admin@ballsrus.ca", "1234567890", 752.99m);
            AddOrder(builder, address, adminUser, new List<Product> { productVPA1989PUM, productAGP3890SHS }, "KJ3H328UYDHS2RFS", "Admin", "Admin", OrderStatus.Confirmed, "admin@ballsrus.ca", "1234567890", 126.99m);
            #endregion
        }

        #region Methods
        /// <summary>
        /// Méthode qui ajoute un utilisateur à un rôle dans la table de liaison de la base de données.
        /// </summary>
        /// <param name="user">L'utilisateur à lier à un rôle</param>
        /// <param name="role">Le rôle qu'on souhaite lui donner</param>
        private static void AddUserToRole(ModelBuilder builder, User user, IdentityRole<Guid> role)
        {
            builder.Entity<IdentityUserRole<Guid>>().HasData(new IdentityUserRole<Guid>
            {
                UserId = user.Id,
                RoleId = role.Id,
            });
        }

        /// <summary>
        /// Méthode qui crée un rôle dans la base de données.
        /// </summary>
        /// <param name="name">Nom du rôle</param>
        /// <returns>Le rôle créé</returns>
        private static IdentityRole<Guid> AddRole(ModelBuilder builder, string name)
        {
            IdentityRole<Guid> newRole = new()
            {
                Id = Guid.NewGuid(),
                Name = name,
                NormalizedName = name.ToUpper()
            };
            builder.Entity<IdentityRole<Guid>>().HasData(newRole);

            return newRole;
        }

        /// <summary>
        /// Méthode qui crée un utilisateur dans la base de données.
        /// </summary>
        /// <param name="userName">Nom d'utilisateur</param>
        /// <param name="password">Mot de passe</param>
        /// <returns>L'utilisateur créé</returns>
        private static User AddUser(ModelBuilder builder, string email, string password, string firstName, string lastName, string phone)
        {
            User newUser = new(email)
            {
                Id = Guid.NewGuid(),
                NormalizedUserName = email.ToUpper(),
                SecurityStamp = Guid.NewGuid().ToString(),
                Email = email,
                NormalizedEmail = email.ToUpper(),
                FirstName = firstName,
                LastName = lastName,
                PhoneNumber = phone
            };
            newUser.PasswordHash = _passwordHasher.HashPassword(newUser, password);
            builder.Entity<User>().HasData(newUser);

            return newUser;
        }

        /// <summary>
        /// Méthode qui crée une catégorie dans la base de données.
        /// </summary>
        /// <param name="name">Nom</param>
        /// <param name="desc">Description</param>
        /// <param name="imagePath">Chemin vers le fichier image</param>
        /// <param name="creationDate">Date de création</param>
        /// <param name="isArchived">Est une catégorie archivée</param>
        /// <returns>La catégorie créée</returns>
        private static Category AddCategory(ModelBuilder builder, string name, string desc, string imagePath, DateTime creationDate, Guid? parentCategoryId = null, bool isArchived = false)
        {
            Category newCategory = new()
            {
                Id = Guid.NewGuid(),
                Name = name,
                Description = desc,
                ImagePath = imagePath,
                CreationDate = creationDate,
                UpdatedDate = creationDate,
                ParentCategoryId = parentCategoryId,
                IsArchived = isArchived
            };
            builder.Entity<Category>().HasData(newCategory);

            return newCategory;
        }

        /// <summary>
        /// Méthode qui crée un produit dans la base de données.
        /// </summary>
        /// <param name="sku">Numéro unique composé de 10 caractères</param>
        /// <param name="name">Nom</param>
        /// <param name="brand">Marque</param>
        /// <param name="model">Modèle</param>
        /// <param name="imagePath">Chemin vers le fichier image</param>
        /// <param name="shortDesc">Description courte</param>
        /// <param name="fullDesc">Description complète</param>
        /// <param name="weight">Poids en grammes</param>
        /// <param name="size">Taille de l'article</param>
        /// <param name="quantity">Quantité</param>
        /// <param name="retailPrice">Prix</param>
        /// <param name="publicationDate">Date de publication</param>
        /// <param name="discountedPrice">Prix en rabais</param>
        /// <param name="isArchived">Est-ce un produit archivé</param>
        private static Product AddProduct(ModelBuilder builder, string sku, string name, string brand, string model, string imagePath, string shortDesc, string fullDesc,
            int? weight, string? size, int quantity, decimal retailPrice, DateTime publicationDate, decimal? discountedPrice = null, bool isArchived = false)
        {
            Product newProduct = new()
            {
                Id = Guid.NewGuid(),
                SKU = sku,
                Name = name,
                Brand = brand,
                Model = model,
                ImagePath = imagePath,
                ShortDescription = shortDesc,
                FullDescription = fullDesc,
                WeightInGrams = weight,
                Size = size,
                Quantity = quantity,
                RetailPrice = retailPrice,
                PublicationDate = publicationDate,
                LastUpdated = publicationDate,
                DiscountedPrice = discountedPrice,
                IsArchived = isArchived
            };
            builder.Entity<Product>().HasData(newProduct);

            return newProduct;
        }

        /// <summary>
        /// Méthode qui ajoute un produit à une catégorie dans la table de liaison ProductCategory de la base de données.
        /// </summary>
        /// <param name="category">La catégorie à lier à un produit</param>
        /// <param name="product">Le produit à lier à une catégorie</param>
        private static void AddProductToCategory(ModelBuilder builder, Category category, Product product)
        {
            builder.Entity<ProductCategory>().HasData(new ProductCategory
            {
                ProductId = product.Id,
                CategoryId = category.Id
            });
        }

        private static Address AddAddressToUser(ModelBuilder builder, User user, string street, string city, string provinceState,
            string country, string postalCode)
        {
            Address address = new()
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                Street = street,
                City = city,
                StateProvince = provinceState,
                Country = country,
                PostalCode = postalCode
            };

            builder.Entity<Address>().HasData(address);

            return address;
        }

        private static void AddOrder(ModelBuilder builder, Address address, User user, List<Product> products, string number, string firstName,
            string lastName, OrderStatus status, string email, string phone, decimal price)
        {
            Guid orderId = Guid.NewGuid();

            Order order = new()
            {
                UserId = user.Id,
                AddressId = address.Id,
                Id = orderId,
                Status = status,
                Number = number,
                FirstName = firstName,
                LastName = lastName,
                EmailAddress = email,
                PhoneNumber = phone,
                ProductQuantity = products.Count(),
                ProductsCost = price,
                ShippingCost = 9.99m,
                SubTotal = price + 9.99m,
                Taxes = (price + 9.99m) * Constants.TAXES_PERCENTAGE,
                Total = (price + 9.99m) * 1.14975m,
                CreationDate = DateTime.UtcNow
            };

            builder.Entity<Order>().HasData(order);

            foreach (Product p in products)
            {
                OrderItem oi = new OrderItem()
                {
                    Id = Guid.NewGuid(),
                    Quantity = 1,
                    UnitaryPrice = (decimal)p.RetailPrice!,
                    TotalCost = (decimal)p.RetailPrice!,
                    CreationDate = DateTime.UtcNow,
                    OrderId = orderId,
                    ProductId = p.Id
                };

                builder.Entity<OrderItem>().HasData(oi);
            }
        }
        #endregion
    }
}
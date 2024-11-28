# Balls R Us - Plateforme de Commerce Électronique pour Équipement de Golf

[![en](https://img.shields.io/badge/lang-en-red.svg)](https://github.com/ocharron/BallsRUs/blob/master/README.md)
[![fr](https://img.shields.io/badge/lang-fr-blue.svg)](https://github.com/ocharron/BallsRUs/blob/master/README_fr.md)

Bienvenue sur Balls R Us, une plateforme (fictive) de commerce électronique spécialisée dans la vente d'équipement de golf de haute qualité. Que vous soyez un amateur passionné ou un professionnel expérimenté, Balls R Us offre une gamme variée de produits pour répondre à tous vos besoins sur le parcours de golf.

---

## Technologies Utilisées

- **Framework**: ASP.NET Core (.NET 8.0)
- **Base de Données**: Microsoft SQL Server 2022
- **ORM**: Entity Framework Core
- **Paiements**: Stripe

---

## Installation

Pour installer et exécuter Balls R Us sur votre machine locale, veuillez suivre ces étapes :

### Prérequis

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Visual Studio](https://visualstudio.microsoft.com/downloads/) ou un éditeur de texte de votre choix (Visual Studio Code, etc.)
- Microsoft SQL Server 2022 (ou version ultérieure)
- Clé d'API Stripe

### Instructions

1. **Cloner le Référentiel**
   ```bash
   git clone https://github.com/ocharron/BallsRUs.git
   ```

2. **Configuration de la Base de Données**
   - Créez une nouvelle base de données dans SQL Server.
   - Mettez à jour la chaîne de connexion dans le fichier `appsettings.json` avec les détails de votre base de données.
   - Exécutez ensuite une migration et une mise à jour de la base de données. (Code First)

3. **Configuration de Stripe**
   - Obtenez une clé d'API Stripe (clé secrète) depuis le tableau de bord Stripe.
   - Ajoutez cette clé d'API dans le fichier `appsettings.json`.

4. **Compilation et Exécution**
   - Ouvrez le projet dans Visual Studio ou utilisez la ligne de commande.
   - Exécutez la commande suivante pour restaurer les dépendances :
     ```bash
     dotnet restore
     ```
   - Ensuite, exécutez l'application :
     ```bash
     dotnet run
     ```

---

## Fonctionnalités Principales

1. **Catalogue de Produits**: Parcourez une large gamme d'équipements de golf, y compris des clubs, des balles, des sacs, des chaussures, etc.
2. **Gestion du Panier**: Ajoutez des produits à votre panier et gérez-les facilement avant de passer à la caisse.
3. **Paiement Sécurisé**: Utilisation de Stripe pour des transactions de paiement sûres et sécurisées.
4. **Gestion des Utilisateurs**: Enregistrez-vous, connectez-vous et gérez votre profil utilisateur pour une expérience personnalisée.
5. **Système de gestion de contenu**: Gérer tout le catalogue, les produits et les utilisateurs à partir d'un système de gestion de contenu.

---

## Contribution

Les contributions sont les bienvenues! Si vous souhaitez utiliser Balls R Us, veuillez suivre ces étapes :

1. Effectuez un Fork du projet
2. Créez une branche pour votre fonctionnalité (`git checkout -b feature/NomDeVotreFonctionnalité`)
3. Validez vos modifications (`git commit -m 'Ajout d'une nouvelle fonctionnalité'`)
4. Poussez vers la branche (`git push origin feature/NomDeVotreFonctionnalité`)
5. Soumettez une demande d'extraction

---

## Auteur

Ce projet a été développé par [Olivier Charron](https://github.com/ocharron), [Xavier Côté-Daviau](https://github.com/xavcd), [Mahdi Ellili](https://github.com/mahdilili) et [Michael Meilleur](https://github.com/MichaelMeilleur).

---

## Avertissement

Il s'agit d'un projet fictif créé uniquement à des fins de démonstration et pédagogiques. Toute ressemblance avec des produits, services ou entreprises réels est purement fortuite. Plusieurs médias utilisés sur l'application ne sont pas libres de droits.
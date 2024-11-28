namespace BallsRUs.Utilities
{
    public static class Constants
    {
        public const string NA = "N/A";

        // Shopping Cart
        public const decimal TAXES_PERCENTAGE = 0.14975m;
        public const decimal ESTIMATED_SHIPPING_COST = 9.99m;

        // Session keys
        public const string SHOPPING_CART_SESSION_KEY = "ShoppingCartId";

        // Nombre d'occurences
        public const int NB_OF_SHOWCASED_PRODUCTS = 4;

        // Génération des numéros de commande
        public const string CHARACTERS_BANK = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        // Types de tri
        public const string PRICE_HIGH_TO_LOW = "high-to-low";
        public const string PRICE_LOW_TO_HIGH = "low-to-high";
        public const string BRAND_ALPHABETICAL = "brand-alphabetical";
        public const string RELEASE_NEW_TO_OLD = "new-to-old";

        // Pour les ViewModels
        public const int PRODUCT_MIN_NAME_LENGTH = 5;
        public const int PRODUCT_MAX_NAME_LENGTH = 100;
        public const int PRODUCT_MIN_SHORT_DESC_LENGTH = 20;
        public const int PRODUCT_MAX_SHORT_DESC_LENGTH = 200;
        public const int PRODUCT_MIN_FULL_DESC_LENGTH = 100;
        public const int PRODUCT_MAX_FULL_DESC_LENGTH = 1200;
        public const string POSTAL_CODE_REGEX = "^[A-Za-z]\\d[A-Za-z] \\d[A-Za-z]\\d$|^[A-Za-z]\\d[A-Za-z]\\d[A-Za-z]\\d$";
        public const string PRODUCT_SKU_REGEX = "^[A-Z]{3}\\d{4}[A-Z]{3}$";
        public const string PHONE_NUMBER_REGEX = "^[0-9]+$";

        // Roles
        public const string ROLE_UTILISATEUR = "Utilisateur";
        public const string ROLE_ADMIN = "Administrateur";
    }
}

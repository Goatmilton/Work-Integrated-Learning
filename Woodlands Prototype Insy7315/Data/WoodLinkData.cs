using Woodlands_Prototype_Insy7315.Models;

namespace Woodlands_Prototype_Insy7315.Data
{
    public static class WoodLinkData
    {
        public static readonly Dictionary<string, string> SlugToCategory = new()
        {
            { "kitchen-units", "Kitchen Units" },
            { "tv-stands", "TV Stands" },
            { "built-in-cupboards", "Built-In Cupboards" },
            { "cutting-edging", "Cutting & Edging" },
        };

        public static readonly Dictionary<string, string> CategoryToSlug = new()
        {
            { "Kitchen Units", "kitchen-units" },
            { "TV Stands", "tv-stands" },
            { "Built-In Cupboards", "built-in-cupboards" },
            { "Cutting & Edging", "cutting-edging" },
        };

        public static List<Product> Products => new()
        {
            new Product {
                Id = "modern-kitchen-suite", Category = "Kitchen Units", Title = "Modern Kitchen Suite",
                Tagline = "Sleek lines. Enduring quality.",
                Description = "Contemporary kitchen units crafted with premium PG Bison melamine boards. Fully customised to your kitchen dimensions and chosen finishes — from handle-less slab doors to classic shaker profiles. Our installation team handles everything from delivery to final fitting.",
                Image = "/images/products/Kitchen Unit 12.jpeg",
                Gallery = new() { "/images/products/Kitchen Unit 12.jpeg", "/images/products/Kitchen Unit 7.jpeg", "/images/products/Kitchen Unit 10.jpeg" },
                Features = new() { "Floor-to-ceiling cabinets", "Soft-close hinges & drawers", "Custom island options", "Integrated appliance housing", "15+ colour finishes" },
                Finishes = new() { "Arctic White", "Graphite Matt", "Woodgrain Oak", "Concrete Grey", "Gloss White" },
                LeadTime = "2–4 weeks", Tag = "Popular", Price = "From R8,500",
            },
            new Product {
                Id = "curved-luxury-kitchen", Category = "Kitchen Units", Title = "Curved Luxury Kitchen",
                Tagline = "Flowing forms, flawless finish.",
                Description = "Curved kitchen cabinetry that redefines what is possible with PG Bison board materials. CNC precision-cutting allows us to achieve gentle arcs and radius profiles that transform a kitchen into a design centrepiece. Available in any PG Bison melamine colour.",
                Image = "/images/products/Kitchen Unit 6.jpeg",
                Gallery = new() { "/images/products/Kitchen Unit 6.jpeg", "/images/products/Kitchen Unit 9.jpeg", "/images/products/Kitchen Unit 11.jpeg" },
                Features = new() { "Curved radius profiles", "Integrated appliance housing", "Marble-effect finishes", "LED under-cabinet lighting", "Bespoke island designs" },
                Finishes = new() { "Pearl White", "Warm Linen", "Deep Navy", "Sage Green", "Terrazzo Effect" },
                LeadTime = "3–5 weeks", Price = "From R14,000",
            },
            new Product {
                Id = "compact-galley-kitchen", Category = "Kitchen Units", Title = "Compact Galley Kitchen",
                Tagline = "Maximum storage, minimal footprint.",
                Description = "Purpose-built for narrow or apartment kitchens. Every centimetre is optimised — pull-out pantry columns, slim-line overhead units, and deep base drawers eliminate wasted space without sacrificing style.",
                Image = "/images/products/Kitchen Unit 4.jpeg",
                Gallery = new() { "/images/products/Kitchen Unit 4.jpeg", "/images/products/Kitchen Unit 3.jpeg", "/images/products/Kitchen 2.jpeg" },
                Features = new() { "Pull-out pantry columns", "Overhead storage units", "Deep base drawers", "Corner carousel units", "Space-saving design" },
                Finishes = new() { "White Gloss", "Light Oak", "Anthracite", "Soft Grey" },
                LeadTime = "2–3 weeks", Price = "From R6,200",
            },
            new Product {
                Id = "wall-mounted-tv-unit", Category = "TV Stands", Title = "Wall-Mounted TV Unit",
                Tagline = "Float it. Flaunt it.",
                Description = "Floating TV units that create a clean, contemporary look with integrated cable management channels so no wires are ever visible. Specify your TV size and we engineer the unit to match — including the correct wall-mounting substrate.",
                Image = "/images/products/Floating TV-1.jpeg",
                Gallery = new() { "/images/products/Floating TV-1.jpeg", "/images/products/Floating TV-2.jpeg", "/images/products/Floating TV-4.jpeg" },
                Features = new() { "Hidden cable management", "Wall-mounted / floating", "Open + closed storage", "Custom width up to 3m", "LED strip options" },
                Finishes = new() { "White Matt", "Smoked Oak", "Charcoal", "Walnut Effect", "Bianco" },
                LeadTime = "1–2 weeks", Tag = "New", Price = "From R3,800",
            },
            new Product {
                Id = "entertainment-console", Category = "TV Stands", Title = "Entertainment Console",
                Tagline = "Storage that works as hard as you play.",
                Description = "A floor-standing entertainment centre with deep media shelves, adjustable compartments, and a mix of doors and open bays. Designed to house sound equipment, streaming devices, gaming consoles, and décor in one cohesive unit.",
                Image = "/images/products/Floating TV-8.jpeg",
                Gallery = new() { "/images/products/Floating TV-8.jpeg", "/images/products/Floating TV-9.jpeg", "/images/products/Floating TV-10.jpeg", "/images/products/Floating TV-5.jpeg" },
                Features = new() { "Deep media shelves", "Adjustable compartments", "Door + open bay combo", "Matching wall panels", "Base plinth or leg options" },
                Finishes = new() { "Arctic White", "Teak Effect", "Black Matt", "Light Grey" },
                LeadTime = "1–2 weeks", Price = "From R2,900",
            },
            new Product {
                Id = "full-length-wardrobe", Category = "Built-In Cupboards", Title = "Full-Length Wardrobe",
                Tagline = "Every centimetre, perfectly used.",
                Description = "Floor-to-ceiling built-in wardrobes that make the most of your bedroom height. Choose sliding or hinged doors, full-length mirrors, internal drawer systems, and a mix of hanging and shelving zones. Installed by our own team with minimal disruption.",
                Image = "https://images.unsplash.com/photo-1720087448033-db1904db294b?w=800&h=600&fit=crop&auto=format",
                Gallery = new() {
                    "https://images.unsplash.com/photo-1720087448033-db1904db294b?w=800&h=600&fit=crop&auto=format",
                    "https://images.unsplash.com/photo-1721738857328-3987700892e7?w=800&h=600&fit=crop&auto=format",
                    "https://images.unsplash.com/photo-1721742604074-8e998cee43f2?w=800&h=600&fit=crop&auto=format",
                },
                Features = new() { "Full-length mirror options", "Internal drawer systems", "Hanging + shelving zones", "Soft-close doors", "Floor-to-ceiling height" },
                Finishes = new() { "White", "Sand", "Pewter", "Cashmere", "Stone Grey" },
                LeadTime = "1–2 weeks", Tag = "Popular", Price = "From R4,500",
            },
            new Product {
                Id = "bedroom-suite-storage", Category = "Built-In Cupboards", Title = "Bedroom Suite Storage",
                Tagline = "Coordinated. Considered. Complete.",
                Description = "A fully coordinated bedroom storage solution combining open display shelving, closed cupboards, and drawer pedestals in a unified design. Pair with a matching headboard panel for a truly bespoke bedroom suite.",
                Image = "https://images.unsplash.com/photo-1721738857328-3987700892e7?w=800&h=600&fit=crop&auto=format",
                Gallery = new() {
                    "https://images.unsplash.com/photo-1721738857328-3987700892e7?w=800&h=600&fit=crop&auto=format",
                    "https://images.unsplash.com/photo-1720087448033-db1904db294b?w=800&h=600&fit=crop&auto=format",
                },
                Features = new() { "Modular configuration", "Integrated LED lighting", "Bedside pedestals", "Headboard panel option", "Premium handles & ironmongery" },
                Finishes = new() { "Linen", "Dove White", "Dusty Rose", "Sage", "Midnight Blue" },
                LeadTime = "2–3 weeks", Price = "From R6,800",
            },
            new Product {
                Id = "precision-board-cutting", Category = "Cutting & Edging", Title = "Precision Board Cutting",
                Tagline = "Your measurements. Our precision.",
                Description = "CNC-precision cutting of any PG Bison chipboard, MDF, or melamine board to your exact specifications. Ideal for contractors, cabinet-makers, and advanced DIY builders. Supply your cut-list and we do the rest — often same day.",
                Image = "https://images.unsplash.com/photo-1659930087003-2d64e33181f7?w=800&h=600&fit=crop&auto=format",
                Gallery = new() {
                    "https://images.unsplash.com/photo-1659930087003-2d64e33181f7?w=800&h=600&fit=crop&auto=format",
                    "https://images.unsplash.com/photo-1497219055242-93359eeed651?w=800&h=600&fit=crop&auto=format",
                },
                Features = new() { "±0.5mm tolerance", "All PG Bison board ranges", "Same-day turnaround (bulk orders)", "Digital cut-list accepted", "Bulk discount pricing" },
                Finishes = new() { "All PG Bison melamine colours", "Raw chipboard", "MDF", "Supawood" },
                LeadTime = "Same day – 2 days", Price = "From R18/cut",
            },
            new Product {
                Id = "edge-banding-finishing", Category = "Cutting & Edging", Title = "Edge Banding & Finishing",
                Tagline = "The detail that defines the finish.",
                Description = "Professional PVC and ABS edge banding applied with a hot-melt adhesive press and trimmed flush — colour-matched from the full PG Bison ABS edging range. Available on any board thickness from 16mm to 38mm.",
                Image = "https://images.unsplash.com/photo-1497219055242-93359eeed651?w=800&h=600&fit=crop&auto=format",
                Gallery = new() {
                    "https://images.unsplash.com/photo-1497219055242-93359eeed651?w=800&h=600&fit=crop&auto=format",
                    "https://images.unsplash.com/photo-1659930087003-2d64e33181f7?w=800&h=600&fit=crop&auto=format",
                },
                Features = new() { "PVC & ABS edging", "Full colour-match range", "Flush trim finish", "16mm–38mm boards", "Bulk pricing available" },
                Finishes = new() { "Matched to all PG Bison decors", "Contrasting accent options" },
                LeadTime = "Same day – 2 days", Price = "From R8/linear metre",
            },
        };

        public static List<ProductCategory> Categories => new()
        {
            new ProductCategory { Id = "Kitchen Units", Label = "Kitchen Units", Slug = "kitchen-units", Count = 3,
                Image = "/images/products/Kitchen Unit 12.jpeg", Description = "Custom kitchens crafted in PG Bison melamine" },
            new ProductCategory { Id = "TV Stands", Label = "TV Stands", Slug = "tv-stands", Count = 2,
                Image = "/images/products/Floating TV-1.jpeg", Description = "Wall-mounted and floor-standing entertainment units" },
            new ProductCategory { Id = "Built-In Cupboards", Label = "Built-In Cupboards", Slug = "built-in-cupboards", Count = 2,
                Image = "https://images.unsplash.com/photo-1720087448033-db1904db294b?w=600&h=700&fit=crop&auto=format",
                Description = "Floor-to-ceiling fitted wardrobes and storage" },
            new ProductCategory { Id = "Cutting & Edging", Label = "Cutting & Edging", Slug = "cutting-edging", Count = 2,
                Image = "https://images.unsplash.com/photo-1659930087003-2d64e33181f7?w=600&h=700&fit=crop&auto=format",
                Description = "CNC precision cutting and edge banding services" },
        };

        public static List<HeroSlide> HeroSlides => new()
        {
            new HeroSlide { Id = 1, Heading = "Built to Last.", Accent = "Designed to Impress.",
                Sub = "Premium custom-built kitchen units, TV stands & built-in cupboards. PG Bison certified.",
                Image = "/images/products/Kitchen Unit 12.jpeg", Cta = "Shop Kitchens", Link = "/Products?category=kitchen-units" },
            new HeroSlide { Id = 2, Heading = "Your Bedroom,", Accent = "Perfectly Organised.",
                Sub = "Floor-to-ceiling built-in cupboards made exactly to your space and style.",
                Image = "https://images.unsplash.com/photo-1720087448033-db1904db294b?w=1920&h=900&fit=crop&auto=format",
                Cta = "View Cupboards", Link = "/Products?category=built-in-cupboards" },
            new HeroSlide { Id = 3, Heading = "Precision Cutting.", Accent = "Zero Compromise.",
                Sub = "CNC board cutting and edge banding for contractors, designers & trade clients.",
                Image = "https://images.unsplash.com/photo-1659930087003-2d64e33181f7?w=1920&h=900&fit=crop&auto=format",
                Cta = "Cutting & Edging", Link = "/Products?category=cutting-edging" },
            new HeroSlide { Id = 4, Heading = "Entertainment Spaces", Accent = "Done Right.",
                Sub = "Custom TV units and entertainment consoles — wall-mounted or floor-standing.",
                Image = "/images/products/Floating TV-9.jpeg", Cta = "View TV Stands", Link = "/Products?category=tv-stands" },
        };

    }
}

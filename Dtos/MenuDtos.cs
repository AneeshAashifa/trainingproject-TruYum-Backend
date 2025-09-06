namespace TruYum.Api.Dtos
{
    public record MenuBaseDto(
        int Id,
        string Name,
        string Description,
        decimal Price,
        bool Active,
        DateTime LaunchDate,
        string? ImageUrl,
        bool Veg
    );

    public record BeverageCreateDto(
        string Name,
        string Description,
        decimal Price,
        bool Active,
        DateTime LaunchDate,
        string? ImageUrl,
        bool Veg,
        bool IsIced,
        double VolumeMl
    );

    public record StarterCreateDto(
        string Name,
        string Description,
        decimal Price,
        bool Active,
        DateTime LaunchDate,
        string? ImageUrl,
        bool Veg,
        bool Spicy,
        int Serves
    );

    public record MainCourseCreateDto(
        string Name,
        string Description,
        decimal Price,
        bool Active,
        DateTime LaunchDate,
        string? ImageUrl,
        bool Veg,
        string? Cuisine,
        string? Size
    );

    public record SnackCreateDto(
        string Name,
        string Description,
        decimal Price,
        bool Active,
        DateTime LaunchDate,
        string? ImageUrl,
        bool Veg,
        bool Baked
    );
}

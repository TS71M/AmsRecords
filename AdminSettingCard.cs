namespace Records;

public record SettingCard(string Title, string Description, string Area, string Page)
{
    public SettingCard(string title, string description, string area)
       : this(title, description, area, title.Replace(" ", string.Empty)) { }
};

public record SettingCardIndex
{
    public string Title { get; init; }
    public string Description { get; init; }
    public string Area { get; init; }
    public string Page { get; init; }

    public SettingCardIndex(string title, string description)
    {
        Title = title;
        Description = description;

        var slug = title.Replace(" ", string.Empty);
        Area = slug;
        Page = $"{slug}Index";
    }

    public SettingCardIndex(string title, string description, string area, string page)
    {
        Title = title;
        Description = description;
        Area = area;
        Page = page;
    }
}

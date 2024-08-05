namespace HotelBookingWebsite.Models.Public
{
    //Tao generic cho booking model ()
    public readonly record struct LookUpModel<TId>(TId Id,string Name);

    public readonly record struct LookUpModel<TId,TAdditionalData>(TId Id, string Name,TAdditionalData AdditionalData)
    {
        public bool IsEmpty => string.IsNullOrWhiteSpace(Name);
    }
}

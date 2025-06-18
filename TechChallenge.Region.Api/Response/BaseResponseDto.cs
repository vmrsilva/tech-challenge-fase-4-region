namespace TechChallenge.Region.Api.Response
{
    public record BaseResponseDto<T> : BaseResponse
    {

        public T Data { get; init; }
    }
}

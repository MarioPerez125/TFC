
namespace TFC.AppEventos.Transversal.Utils { 
    public class BaseResponse
    {
        public bool IsSuccess { get; set; }
        public string? Message { get; set; }
        public ResponseCodes ResponseCode { get; set; }
    }
}
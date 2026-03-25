namespace SocialCrap.Service
{
    // Resultado padrao para operacoes sem retorno de dados.
    public class OperationResult
    {
        public bool Success { get; set; }
        public bool NotFound { get; set; }
        public string? Error { get; set; }

        // Atalho para sucesso.
        public static OperationResult Ok() => new() { Success = true };
        // Atalho para nao encontrado.
        public static OperationResult NotFoundResult(string? error = null) => new() { Success = false, NotFound = true, Error = error };
        // Atalho para falha com mensagem.
        public static OperationResult Fail(string error) => new() { Success = false, Error = error };
    }

    // Resultado padrao para operacoes que retornam dados.
    public class OperationResult<T>
    {
        public bool Success { get; set; }
        public bool NotFound { get; set; }
        public string? Error { get; set; }
        public T? Data { get; set; }

        // Atalho para sucesso com dados.
        public static OperationResult<T> Ok(T data) => new() { Success = true, Data = data };
        // Atalho para nao encontrado.
        public static OperationResult<T> NotFoundResult(string? error = null) => new() { Success = false, NotFound = true, Error = error };
        // Atalho para falha com mensagem.
        public static OperationResult<T> Fail(string error) => new() { Success = false, Error = error };
    }
}

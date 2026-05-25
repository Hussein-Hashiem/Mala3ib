namespace Mala3ib.BLL.Errors
{
    public static class PlayerErrors
    {
        public static Error NotFound
            = new Error("Player.NotFound", "Player not found", ErrorType.NotFound);
    }
}

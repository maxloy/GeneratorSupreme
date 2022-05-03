namespace NGT_Web.Shared
{
	public static class ModalManager
	{
		public static string Header { get; private set; } = "";
		public static string Body { get; private set; } = "";
		public static Action Callback { get; private set; } = () => { };

		public static event EventHandler ModalsNeedUpdating = (obj, args) => { };

		public static void SetModalData(string modalHeader, string modalBody, Action callback)
		{
			Header = modalHeader;
			Body = modalBody;
			Callback = callback;
			ModalsNeedUpdating.Invoke(null, EventArgs.Empty);
		}
	}
}

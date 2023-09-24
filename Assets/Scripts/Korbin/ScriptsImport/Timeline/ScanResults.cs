namespace Timeline
{
	public struct ScanResults
	{
		public bool AnythingScanned;
		public ShipEvent ScannedEvent;
		public int BeatsUntilEvent;//1 would be the next event.
	}
}
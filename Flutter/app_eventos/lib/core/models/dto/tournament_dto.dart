class TournamentDto {
  final int tournamentId;
  final String location;
  final String arena;
  final String startDate;
  final String endDate;
  final String sportType;
  final int organizerId;

  TournamentDto({
    required this.tournamentId,
    required this.location,
    required this.arena,
    required this.startDate,
    required this.endDate,
    required this.sportType,
    required this.organizerId,
  });

  factory TournamentDto.fromJson(Map<String, dynamic> json) => TournamentDto(
    tournamentId: json['tournamentId'],
    location: json['location'],
    startDate: json['startDate'],
    arena: json['arena'],
    endDate: json['endDate'],
    sportType: json['sportType'],
    organizerId: json['organizerId'],
  );
}
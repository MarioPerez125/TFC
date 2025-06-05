class FightDto {
  final int fightId;
  final int tournamentId;
  final int fighter1Id;
  final int fighter2Id;
  final String? status;
  final int? winnerId;
  final String? nombrePeleador1;
  final String? nombrePeleador2;

  FightDto({
    required this.fightId,
    required this.tournamentId,
    required this.fighter1Id,
    required this.fighter2Id,
    this.status,
    this.winnerId,
    this.nombrePeleador1,
    this.nombrePeleador2,
  });

  factory FightDto.fromJson(Map<String, dynamic> json) => FightDto(
        fightId: json['fightId'],
        tournamentId: json['tournamentId'],
        fighter1Id: json['fighter1Id'],
        fighter2Id: json['fighter2Id'],
        status: json['status'],
        winnerId: json['winnerId'],
        nombrePeleador1: json['nombrePeleador1'],
        nombrePeleador2: json['nombrePeleador2'],
      );
}
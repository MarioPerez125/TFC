class FighterDto {
  final int userId;
  final String weightClass;
  final int height;
  final int reach;
  final int wins;
  final int losses;
  final int draws;

  FighterDto({
    required this.userId,
    required this.weightClass,
    required this.height,
    required this.reach,
    required this.wins,
    required this.losses,
    required this.draws,
  });

  factory FighterDto.fromJson(Map<String, dynamic> json) => FighterDto(
    userId: json['userId'],
    weightClass: json['weightClass'],
    height: json['height'],
    reach: json['reach'],
    wins: json['wins'],
    losses: json['losses'],
    draws: json['draws'],
  );

  Map<String, dynamic> toJson() {
    return {
      'userId': userId,
      'weightClass': weightClass,
      'height': height,
      'reach': reach,
      'wins': wins,
      'losses': losses,
      'draws': draws,
    };
  }
}
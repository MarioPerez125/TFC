class FighterForFriendDto {
  final int? userId;
  final String username;
  final String email;
  final String name;
  final String lastName;
  final int? phone;
  final String birthDate;
  final String city;
  final String country;
  final String role;
  final int fighterId;
  final int wins;
  final int losses;
  final int draws;
  final String weightClass;
  final int? height;
  final int? reach;

  FighterForFriendDto({
    this.userId,
    required this.username,
    required this.email,
    required this.name,
    required this.lastName,
    this.phone,
    required this.birthDate,
    required this.city,
    required this.country,
    required this.role,
    required this.fighterId,
    required this.wins,
    required this.losses,
    required this.draws,
    required this.weightClass,
    this.height,
    this.reach,
  });

  factory FighterForFriendDto.fromJson(Map<String, dynamic> json) =>
      FighterForFriendDto(
        userId: json['userId'],
        username: json['username'],
        email: json['email'],
        name: json['name'],
        lastName: json['lastName'],
        phone: json['phone'],
        birthDate: json['birthDate'],
        city: json['city'],
        country: json['country'],
        role: json['role'],
        fighterId: json['fighterId'],
        wins: json['wins'],
        losses: json['losses'],
        draws: json['draws'],
        weightClass: json['weightClass'],
        height: json['height'],
        reach: json['reach'],
      );
}
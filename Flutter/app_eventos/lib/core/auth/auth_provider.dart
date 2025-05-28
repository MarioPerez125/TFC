import 'package:app_eventos/core/models/dto/register_dto.dart';
import 'package:flutter/foundation.dart';
import '../models/user_model.dart';
import 'auth_service.dart';

class AuthProvider with ChangeNotifier {
  final AuthService _authService;
  User? _user;
  bool _isLoading = false;

  AuthProvider(this._authService);

  User? get user => _user;
  bool get isLoading => _isLoading;

  Future<void> initialize() async {
    final userDto = await _authService.getCurrentUser();
    _user = userDto != null ? User.fromDto(userDto) : null;
    notifyListeners();
  }

  Future<User?> loginWithEmail(String email, String password, String username) async {
    _isLoading = true;
    notifyListeners();

    try {
      final userDto = await _authService.loginWithEmail(email, password, username);
      _user = userDto != null ? User.fromDto(userDto) : null;
      return _user;
    } finally {
      _isLoading = false;
      notifyListeners();
    }
  }

  Future<RegisterDto?> register(
    String name,
    String lastName,
    String email,
    String password,
    String username,
    int phone,
    String birthDate,
    String city,
    String country,
  ) async {
    _isLoading = true;
    notifyListeners();
    try {
      final registerDto = await _authService.register(
        name: name,
        lastName: lastName,
        email: email,
        password: password,
        username: username,
        phone: phone,
        birthDate: birthDate,
        city: city,
        country: country,
      );
      return registerDto;
    } finally {
      _isLoading = false;
      notifyListeners();
    }
  }

  Future<void> logout() async {
    await _authService.logout();
    _user = null;
    notifyListeners();
  }

  Future<void> refreshUser() async {
    final userDto = await _authService.getCurrentUser();
    _user = userDto != null ? User.fromDto(userDto) : null;
    notifyListeners();
  }
}
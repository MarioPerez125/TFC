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

  Future<bool> register(
    String name,
    String lastName,
    String email,
    String password,
    String username, // <-- username va en 5to lugar
    {int? phone, String? birthDate, String? city, String? country}
  ) async {
    _isLoading = true;
    notifyListeners();
    try {
      final result = await _authService.register(
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
      return result;
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
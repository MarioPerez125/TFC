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

// ...existing code...

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

  Future<void> logout() async {
    await _authService.logout();
    _user = null;
    notifyListeners();
  }
}
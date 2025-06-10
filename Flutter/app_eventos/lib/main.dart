import 'package:app_eventos/core/auth/service.dart';
import 'package:app_eventos/features/auth/screens/login_screen.dart';
import 'package:app_eventos/features/auth/screens/main_screen.dart';
import 'package:flutter/material.dart';
import 'package:flutter_localizations/flutter_localizations.dart';
import 'package:provider/provider.dart';
import 'package:flutter_secure_storage/flutter_secure_storage.dart';
import 'package:google_fonts/google_fonts.dart';

import '../../core/auth/auth_provider.dart';

void main() async {
  WidgetsFlutterBinding.ensureInitialized();

  final storage = FlutterSecureStorage();
  final token = await storage.read(key: 'jwt_token');
  final isLoggedIn = token != null;

  final authProvider = AuthProvider(Service());
  await authProvider.initialize();

  runApp(
    MultiProvider(
      providers: [
        ChangeNotifierProvider.value(
          value: authProvider,
        ),
      ],
      child: MyApp(isLoggedIn: isLoggedIn),
    ),
  );
}

class MyApp extends StatelessWidget {
  final bool isLoggedIn;

  const MyApp({super.key, required this.isLoggedIn});

  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      title: 'Fans de Deportes de Contacto',
      home: isLoggedIn ? const MainScreen() : const LoginScreen(),
      routes: {
        '/login': (context) => const LoginScreen(),
        '/main': (context) => const MainScreen(),
      },
      theme: ThemeData(
        colorScheme: ColorScheme.fromSeed(seedColor: Colors.deepPurple),
        scaffoldBackgroundColor: const Color(0xFFF6F7FB),
        fontFamily: GoogleFonts.montserrat().fontFamily,
        appBarTheme: const AppBarTheme(
          backgroundColor: Colors.transparent,
          elevation: 0,
          foregroundColor: Colors.deepPurple,
          titleTextStyle: TextStyle(fontWeight: FontWeight.bold, fontSize: 22, color: Colors.deepPurple),
        ),
        inputDecorationTheme: InputDecorationTheme(
          filled: true,
          fillColor: Colors.white,
          border: OutlineInputBorder(
            borderRadius: BorderRadius.all(Radius.circular(18)),
            borderSide: BorderSide(color: Colors.deepPurple, width: 1.5),
          ),
          enabledBorder: OutlineInputBorder(
            borderRadius: BorderRadius.all(Radius.circular(18)),
            borderSide: BorderSide(color: Colors.deepPurple),
          ),
          focusedBorder: OutlineInputBorder(
            borderRadius: BorderRadius.all(Radius.circular(18)),
            borderSide: BorderSide(color: Colors.deepPurple, width: 2),
          ),
          labelStyle: TextStyle(color: Colors.deepPurple),
          prefixIconColor: Colors.deepPurple,
        ),
        elevatedButtonTheme: ElevatedButtonThemeData(
          style: ElevatedButton.styleFrom(
            backgroundColor: Colors.deepPurple,
            foregroundColor: Colors.white,
            shape: RoundedRectangleBorder(
              borderRadius: BorderRadius.circular(18),
            ),
            padding: const EdgeInsets.symmetric(vertical: 16),
            textStyle: const TextStyle(fontWeight: FontWeight.bold, fontSize: 16),
          ),
        ),
        textButtonTheme: TextButtonThemeData(
          style: TextButton.styleFrom(
            foregroundColor: Colors.deepPurple,
            textStyle: const TextStyle(fontWeight: FontWeight.w500),
          ),
        ),
        snackBarTheme: const SnackBarThemeData(
          backgroundColor: Colors.deepPurple,
          contentTextStyle: TextStyle(color: Colors.white),
          behavior: SnackBarBehavior.floating,
          shape: RoundedRectangleBorder(
            borderRadius: BorderRadius.all(Radius.circular(12)),
          ),
        ),
      ),
      debugShowCheckedModeBanner: false,
      localizationsDelegates: const [
        GlobalMaterialLocalizations.delegate,
        GlobalWidgetsLocalizations.delegate,
        GlobalCupertinoLocalizations.delegate,
      ],
    );
  }
}
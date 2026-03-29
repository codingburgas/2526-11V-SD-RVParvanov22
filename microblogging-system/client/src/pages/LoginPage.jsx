function LoginPage() {
  return (
    <div className="container py-5">
      <div className="row justify-content-center">
        <div className="col-12 col-md-8 col-lg-5">
          <div className="glass-card p-4 p-md-5">
            <h2 className="fw-bold mb-2">Welcome back</h2>
            <p className="text-light opacity-75 mb-4">
              Sign in to your PlayerPulse account
            </p>

            <form>
              <div className="mb-3">
                <label className="form-label">Email</label>
                <input type="email" className="form-control" placeholder="Enter your email" />
              </div>

              <div className="mb-4">
                <label className="form-label">Password</label>
                <input type="password" className="form-control" placeholder="Enter your password" />
              </div>

              <button type="submit" className="btn purple-btn w-100 py-2">
                Sign In
              </button>
            </form>
          </div>
        </div>
      </div>
    </div>
  )
}

export default LoginPage
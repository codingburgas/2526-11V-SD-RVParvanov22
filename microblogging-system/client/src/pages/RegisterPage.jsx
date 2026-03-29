function RegisterPage() {
  return (
    <div className="container py-5">
      <div className="row justify-content-center">
        <div className="col-12 col-md-9 col-lg-6">
          <div className="glass-card p-4 p-md-5">
            <h2 className="fw-bold mb-2">Create account</h2>
            <p className="text-light opacity-75 mb-4">
              Build your gaming profile and share your progress
            </p>

            <form>
              <div className="mb-3">
                <label className="form-label">Display Name</label>
                <input type="text" className="form-control" placeholder="Enter display name" />
              </div>

              <div className="mb-3">
                <label className="form-label">Email</label>
                <input type="email" className="form-control" placeholder="Enter your email" />
              </div>

              <div className="mb-3">
                <label className="form-label">Password</label>
                <input type="password" className="form-control" placeholder="Create password" />
              </div>

              <div className="mb-4">
                <label className="form-label">Confirm Password</label>
                <input type="password" className="form-control" placeholder="Confirm password" />
              </div>

              <button type="submit" className="btn purple-btn w-100 py-2">
                Create Account
              </button>
            </form>
          </div>
        </div>
      </div>
    </div>
  )
}

export default RegisterPage
function Sidebar() {
  return (
    <div className="sidebar">
      {/* Logo Section */}
      <div className="sidebar-logo">
        <h1 className="sidebar-title">PlayerPulse</h1>
        <p className="sidebar-subtitle">Gaming scouting platform</p>
      </div>

      {/* Navigation */}
      <nav className="sidebar-nav">
        <div className="nav-item active">
          <i className="bi bi-house-fill"></i>
          <span>Home</span>
        </div>
        <div className="nav-item">
          <i className="bi bi-compass"></i>
          <span>Explore</span>
        </div>
        <div className="nav-item">
          <i className="bi bi-person"></i>
          <span>Profile</span>
        </div>
        <div className="nav-item">
          <i className="bi bi-pencil-square"></i>
          <span>Create Post</span>
        </div>
        <div className="nav-item">
          <i className="bi bi-file-text"></i>
          <span>Reports</span>
        </div>
      </nav>

      {/* Create Post Button */}
      <button className="btn purple-btn w-100 mb-4 py-2 fw-bold">
        <i className="bi bi-plus-lg"></i> Create Post
      </button>

      {/* User Card at Bottom */}
      <div className="user-card">
        <div className="user-avatar">
          <div className="avatar-circle" style={{backgroundImage: 'url(https://api.dicebear.com/7.x/avataaars/svg?seed=rumen)'}}>
          </div>
        </div>
        <div className="user-info">
          <div className="user-name">You</div>
          <div className="user-role">Immortal Valorant Player</div>
        </div>
      </div>
    </div>
  )
}

export default Sidebar
